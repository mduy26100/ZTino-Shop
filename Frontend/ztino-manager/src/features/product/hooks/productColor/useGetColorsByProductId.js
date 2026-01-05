import { useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios'; 
import { getColorsByProductId } from '../../api';

const CACHE = {}; 

const CACHE_TTL = 5 * 60 * 1000;

export const useGetColorsByProductId = (productId, options = {}) => {
    const { onSuccess, onError } = options;
    
    const cachedEntry = CACHE[productId];
    const [data, setData] = useState(cachedEntry?.data || null);
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
        if (!productId) return;

        const now = Date.now();
        const currentOnSuccess = fetchOptions.onSuccess || onSuccess;
        const currentOnError = fetchOptions.onError || onError;
        const currentCache = CACHE[productId];

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
                    currentOnSuccess?.(res);
                }
            } catch (err) {
                if (isMounted.current) {
                    setError(err);
                    currentOnError?.(err);
                }
            } finally {
                if (isMounted.current) setIsLoading(false);
            }
            return;
        }

        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }
        
        const controller = new AbortController();
        abortControllerRef.current = controller;

        if (isMounted.current) {
            if (!CACHE[productId]?.data) {
                setIsLoading(true);
            }
            setError(null);
        }

        try {
            const promise = getColorsByProductId(productId, { signal: controller.signal });
            
            if (!CACHE[productId]) CACHE[productId] = {};
            CACHE[productId].promise = promise;
            
            const response = await promise;
            const rawData = response?.data || response; 
            
            CACHE[productId] = {
                data: rawData,
                timestamp: Date.now(),
                promise: null
            };

            if (isMounted.current) {
                setData(rawData);
                currentOnSuccess?.(rawData);
            }
        } catch (err) {
            if (axios.isCancel(err)) {
                return;
            }

            if (CACHE[productId]) CACHE[productId].promise = null;

            if (isMounted.current) {
                setError(err);
                currentOnError?.(err);
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, [productId, onSuccess, onError]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback((opt) => fetchData(true, opt), [fetchData]);

    const currentCache = CACHE[productId];
    
    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(currentCache?.data && Date.now() - currentCache.timestamp < CACHE_TTL),
    };
};