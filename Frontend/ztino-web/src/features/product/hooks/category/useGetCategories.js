import { useState, useEffect, useCallback, useRef } from 'react';
import { getCategories } from '../../api/category.api';

const CACHE = {
    data: null,
    timestamp: null,
    promise: null,
};

const CACHE_TTL = 5 * 60 * 1000;

export const useGetCategories = () => {
    const [data, setData] = useState(CACHE.data || []);
    const [isLoading, setIsLoading] = useState(!CACHE.data);
    const [error, setError] = useState(null);
    
    const abortControllerRef = useRef(null);

    const fetchData = useCallback(async (forceUpdate = false) => {
        const now = Date.now();
        const isCacheValid = CACHE.data && CACHE.timestamp && (now - CACHE.timestamp < CACHE_TTL);

        if (!forceUpdate && isCacheValid) {
            setData(CACHE.data);
            setIsLoading(false);
            return;
        }

        if (CACHE.promise && !forceUpdate) {
            try {
                const res = await CACHE.promise;
                setData(res);
            } catch (err) {
                setError(err);
            } finally {
                setIsLoading(false);
            }
            return;
        }

        setIsLoading(true);
        setError(null);

        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }
        abortControllerRef.current = new AbortController();

        try {
            const apiCall = getCategories({ signal: abortControllerRef.current.signal });
            CACHE.promise = apiCall;

            const response = await apiCall;
            
            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : [];

            CACHE.data = safeData;
            CACHE.timestamp = Date.now();
            
            setData(safeData);
        } catch (err) {
            if (err.name !== 'AbortError') {
                setError(err);
                CACHE.data = null; 
                CACHE.timestamp = null;
            }
        } finally {
            CACHE.promise = null;
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchData();
        return () => {
            if (abortControllerRef.current) {
                abortControllerRef.current.abort();
            }
        };
    }, [fetchData]);

    const refetch = useCallback(() => fetchData(true), [fetchData]);

    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(CACHE.data && CACHE.timestamp && (Date.now() - CACHE.timestamp < CACHE_TTL)),
    };
};