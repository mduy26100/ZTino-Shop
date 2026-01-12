import { useState, useEffect, useRef, useCallback } from 'react';

const GLOBAL_CACHE = {};

export const clearGlobalCache = () => {
    for (const key in GLOBAL_CACHE) delete GLOBAL_CACHE[key];
};

export const invalidateCache = (key) => {
    if (key) delete GLOBAL_CACHE[key];
};

export const useQuery = (key, queryFn, options = {}) => {
    const {
        ttl = 5 * 60 * 1000,
        enabled = true,
        initialData = null,
    } = options;

    const optionsRef = useRef(options);
    useEffect(() => {
        optionsRef.current = options;
    });

    const getCachedData = useCallback(() => {
        const cached = GLOBAL_CACHE[key];
        if (cached && (Date.now() - cached.timestamp < ttl)) {
            return cached.data;
        }
        return null;
    }, [key, ttl]);

    const [data, setData] = useState(() => getCachedData() ?? initialData);
    const [isLoading, setIsLoading] = useState(enabled && !getCachedData());
    const [error, setError] = useState(null);

    const abortControllerRef = useRef(null);

    const fetchData = useCallback(async (force = false) => {
        if (!enabled || !key) return;

        const cached = getCachedData();
        if (!force && cached) {
            setData(cached);
            setIsLoading(false);
            return;
        }

        if (abortControllerRef.current) {
            abortControllerRef.current.abort();
        }

        abortControllerRef.current = new AbortController();
        const signal = abortControllerRef.current.signal;

        setIsLoading(true);
        setError(null);

        try {
            const response = await queryFn({ signal });

            const transformedData = optionsRef.current.transformResponse
                ? optionsRef.current.transformResponse(response)
                : (response?.data || response);

            GLOBAL_CACHE[key] = {
                data: transformedData,
                timestamp: Date.now(),
            };

            if (!signal.aborted) {
                setData(transformedData);
                optionsRef.current.onSuccess?.(transformedData);
            }
        } catch (err) {
            if (!signal.aborted && err.name !== 'AbortError') {
                setError(err);
                optionsRef.current.onError?.(err);
            }
        } finally {
            if (!signal.aborted) {
                setIsLoading(false);
            }
        }
    }, [key, queryFn, enabled, getCachedData]);

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
        isCached: !!getCachedData(),
    };
};
