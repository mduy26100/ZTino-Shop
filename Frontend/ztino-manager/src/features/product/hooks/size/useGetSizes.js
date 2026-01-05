import { useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios'; 
import { getSizes } from '../../api';

const CACHE = {
    data: null,
    timestamp: null,
    promise: null,
};

const CACHE_TTL = 5 * 60 * 1000;

export const useGetSizes = (options = {}) => {
    const { onSuccess, onError } = options;
    
    const [data, setData] = useState(CACHE.data || []);
    const [isLoading, setIsLoading] = useState(!CACHE.data);
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
        const now = Date.now();
        const currentOnSuccess = fetchOptions.onSuccess || onSuccess;
        const currentOnError = fetchOptions.onError || onError;

        if (!forceUpdate && CACHE.data && CACHE.timestamp && (now - CACHE.timestamp < CACHE_TTL)) {
            if (isMounted.current) {
                setData(CACHE.data);
                setIsLoading(false);
                currentOnSuccess?.(CACHE.data);
            }
            return;
        }

        if (CACHE.promise && !forceUpdate) {
            try {
                const res = await CACHE.promise;
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
            setIsLoading(true);
            setError(null);
        }

        try {
            CACHE.promise = getSizes({ signal: controller.signal });
            
            const response = await CACHE.promise;
            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : [];

            CACHE.data = safeData;
            CACHE.timestamp = Date.now();
            CACHE.promise = null;

            if (isMounted.current) {
                setData(safeData);
                currentOnSuccess?.(safeData);
            }
        } catch (err) {
            if (axios.isCancel(err)) {
                return;
            }

            CACHE.promise = null;
            if (isMounted.current) {
                setError(err);
                currentOnError?.(err);
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, [onSuccess, onError]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback((opt) => fetchData(true, opt), [fetchData]);

    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(CACHE.data && Date.now() - CACHE.timestamp < CACHE_TTL),
    };
};