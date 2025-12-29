import { useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios'; 
import { getProductImagesByProductVariantId } from '../../api/productImage.api';

const CACHE = {}; 
const CACHE_TTL = 5 * 60 * 1000; 

export const useGetProductImages = (variantId, options = {}) => {
    const { onSuccess, onError } = options;
    
    const cachedEntry = CACHE[variantId];
    
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
        if (!variantId) {
            if (isMounted.current) {
                setData([]);
                setIsLoading(false);
            }
            return;
        }

        const now = Date.now();
        const currentOnSuccess = fetchOptions.onSuccess || onSuccess;
        const currentOnError = fetchOptions.onError || onError;
        const currentCache = CACHE[variantId];

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
            if (!CACHE[variantId]?.data) {
                setIsLoading(true);
                setData([]);
            }
            setError(null);
        }

        try {
            const promise = getProductImagesByProductVariantId(variantId, { signal: controller.signal });
            
            if (!CACHE[variantId]) CACHE[variantId] = {};
            CACHE[variantId].promise = promise;
            
            const response = await promise;
            
            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : (rawData?.images || []);

            CACHE[variantId] = {
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

            if (CACHE[variantId]) CACHE[variantId].promise = null;

            if (isMounted.current) {
                setError(err);
                currentOnError?.(err);
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, [variantId, onSuccess, onError]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback((opt) => fetchData(true, opt), [fetchData]);

    const currentCache = CACHE[variantId];
    
    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(currentCache?.data && Date.now() - currentCache.timestamp < CACHE_TTL),
    };
};