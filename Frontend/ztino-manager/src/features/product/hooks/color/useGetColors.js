import { useState, useEffect, useCallback, useRef } from 'react';
import { message } from 'antd';
import { getColors } from '../../api/color.api';

const CACHE = {
    data: null,
    timestamp: null,
    promise: null,
};

const CACHE_TTL = 5 * 60 * 1000;

export const useGetColors = () => {
    const [data, setData] = useState(CACHE.data || []);
    const [isLoading, setIsLoading] = useState(!CACHE.data);
    const [error, setError] = useState(null);
    const isMounted = useRef(true);

    useEffect(() => {
        isMounted.current = true;
        return () => {
            isMounted.current = false;
        };
    }, []);

    const fetchData = useCallback(async (forceUpdate = false) => {
        const now = Date.now();

        if (
            !forceUpdate &&
            CACHE.data &&
            CACHE.timestamp &&
            now - CACHE.timestamp < CACHE_TTL
        ) {
            if (isMounted.current) {
                setData(CACHE.data);
                setIsLoading(false);
            }
            return;
        }

        if (CACHE.promise && !forceUpdate) {
            try {
                const res = await CACHE.promise;
                if (isMounted.current) setData(res);
            } catch (err) {
                if (isMounted.current) setError(err);
            } finally {
                if (isMounted.current) setIsLoading(false);
            }
            return;
        }

        if (isMounted.current) {
            setIsLoading(true);
            setError(null);
        }

        try {
            CACHE.promise = getColors();
            const response = await CACHE.promise;
            
            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : [];

            CACHE.data = safeData;
            CACHE.timestamp = Date.now();
            CACHE.promise = null;

            if (isMounted.current) setData(safeData);
        } catch (err) {
            CACHE.promise = null;
            if (isMounted.current) {
                setError(err);
                message.error(err?.message || "Failed to fetch colors");
            }
        } finally {
            if (isMounted.current) setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchData();
    }, [fetchData]);

    const refetch = useCallback(() => fetchData(true), [fetchData]);

    return {
        data,
        isLoading,
        error,
        refetch,
        isCached: !!(CACHE.data && Date.now() - CACHE.timestamp < CACHE_TTL),
    };
};