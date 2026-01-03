import { useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios'; 
import { getProductImagesByProductColorId } from '../../api/productImage.api';

const CACHE = {}; 
const CACHE_TTL = 5 * 60 * 1000; 

export const useGetProductImages = (productColorId, options = {}) => {
    const { onSuccess, onError } = options;
    
    const cachedEntry = CACHE[productColorId];
    
    const [data, setData] = useState(cachedEntry?.data || []);
    const [isLoading, setIsLoading] = useState(!cachedEntry?.data);
    const [error, setError] = useState(null);
    
    const abortControllerRef = useRef(null);
    const isMounted = useRef(true);

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
            if (abortControllerRef.current) {
                abortControllerRef.current.abort();
            }
        };
    }, []);

    const fetchData = useCallback(async (forceUpdate = false, fetchOptions = {}) => {
        if (!productColorId) {
            if (isMounted.current) {
                setData([]);
                setIsLoading(false);
            }
            return;
        }

        const now = Date.now();
        const currentOnSuccess = fetchOptions.onSuccess || onSuccess;
        const currentOnError = fetchOptions.onError || onError;
        const currentCache = CACHE[productColorId];

        if (!forceUpdate && currentCache?.data && currentCache?.timestamp && (now - currentCache.timestamp < CACHE_TTL)) {
            if (isMounted.current) {
                setData(currentCache.data);
                setIsLoading(false);
                currentOnSuccess?.(currentCache.data);
            }
            return;
        }

        if (currentCache?.promise && !forceUpdate) {
            try {
                const res = await currentCache.promise;
                if (isMounted.current) {
                    setData(res);
                    setIsLoading(false);
                    currentOnSuccess?.(res);
                }
            } catch (err) {
                if (isMounted.current) {
                    setError(err);
                    currentOnError?.(err);
                }
            }
            return;
        }

        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }
        
        const controller = new AbortController();
        abortControllerRef.current = controller;

        if (isMounted.current) {
            if (!CACHE[productColorId]?.data) {
                setIsLoading(true);
                setData([]);
            }
            setError(null);
        }

        try {
            const promise = getProductImagesByProductColorId(productColorId, { signal: controller.signal });
            
            if (!CACHE[productColorId]) CACHE[productColorId] = {};
            CACHE[productColorId].promise = promise;
            
            const response = await promise;
            
            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : (rawData?.images || []);

            CACHE[productColorId] = {
                data: safeData,
                timestamp: Date.now(),
                promise: null
            };

            if (isMounted.current) {
                setData(safeData);
                currentOnSuccess?.(safeData);
            }
        } catch (err) {
            if (axios.isCancel(err)) {
                return;
            }

            if (CACHE[productColorId]) CACHE[productColorId].promise = null;

            if (isMounted.current) {
                setError(err);
                currentOnError?.(err);
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, [productColorId, onSuccess, onError]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback((opt) => fetchData(true, opt), [fetchData]);

    const currentCache = CACHE[productColorId];
    
    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(currentCache?.data && Date.now() - currentCache.timestamp < CACHE_TTL),
    };
};