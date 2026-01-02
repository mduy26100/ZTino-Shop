import { useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios';
import { getProductDetailBySlug } from '../../api/product.api';

const CACHE = {};
const CACHE_TTL = 5 * 60 * 1000;

export const useGetProductDetailBySlug = (slug, options = {}) => {
    const { onSuccess, onError } = options;

    const cachedEntry = CACHE[slug];
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
        if (!slug) return;

        const now = Date.now();
        const currentOnSuccess = fetchOptions.onSuccess || onSuccess;
        const currentOnError = fetchOptions.onError || onError;
        const currentCache = CACHE[slug];

        if (!forceUpdate && currentCache?.data && currentCache?.timestamp && (now - currentCache.timestamp < CACHE_TTL)) {
            if (isMounted.current) {
                setData(currentCache.data);
                setIsLoading(false);
                setError(null);
                currentOnSuccess?.(currentCache.data);
            }
            return;
        }

        if (currentCache?.promise && !forceUpdate) {
            try {
                const res = await currentCache.promise;
                if (isMounted.current) {
                    setData(res);
                    setError(null);
                    currentOnSuccess?.(res);
                }
            } catch (err) {
                if (isMounted.current && !axios.isCancel(err)) {
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
            if (!CACHE[slug]?.data) setIsLoading(true);
            setError(null);
        }

        try {
            const promise = getProductDetailBySlug(slug, { signal: controller.signal });
            
            if (!CACHE[slug]) CACHE[slug] = {};
            CACHE[slug].promise = promise;

            const response = await promise;
            const rawData = response?.data || response;

            CACHE[slug] = {
                data: rawData,
                timestamp: Date.now(),
                promise: null
            };

            if (isMounted.current) {
                setData(rawData);
                currentOnSuccess?.(rawData);
            }
        } catch (err) {
            if (axios.isCancel(err)) return;

            if (CACHE[slug]) CACHE[slug].promise = null;

            if (isMounted.current) {
                setError(err);
                currentOnError?.(err);
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, [slug, onSuccess, onError]);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback((opt) => fetchData(true, opt), [fetchData]);

    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(CACHE[slug]?.data && Date.now() - CACHE[slug].timestamp < CACHE_TTL),
    };
};