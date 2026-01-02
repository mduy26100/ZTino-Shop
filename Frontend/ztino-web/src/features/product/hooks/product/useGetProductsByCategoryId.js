import { useState, useEffect, useCallback } from 'react';
import { getProductsByCategoryId } from '../../api/product.api';

const CACHE = {};
const CACHE_TTL = 5 * 60 * 1000;

const getValidCache = (categoryId) => {
    const cachedItem = CACHE[categoryId];
    if (!cachedItem) return null;

    const isExpired = Date.now() - cachedItem.timestamp > CACHE_TTL;
    return isExpired ? null : cachedItem.data;
};

export const useGetProductsByCategoryId = (categoryId) => {
    const [data, setData] = useState(() => getValidCache(categoryId) || []);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchData = useCallback(async (forceUpdate = false, signalIgnore = { current: false }) => {
        if (!categoryId) {
            setData([]);
            return;
        }

        const validData = getValidCache(categoryId);

        if (!forceUpdate && validData) {
            setData(validData);
            setError(null);
            return;
        }

        setIsLoading(true);
        setError(null);

        try {
            const response = await getProductsByCategoryId(categoryId);
            
            if (signalIgnore.current) return;

            const rawData = response?.data || response;
            const safeData = Array.isArray(rawData) ? rawData : [];

            CACHE[categoryId] = {
                data: safeData,
                timestamp: Date.now()
            };

            setData(safeData);
        } catch (err) {
            if (signalIgnore.current) return;

            setError(err);
            delete CACHE[categoryId];
        } finally {
            if (!signalIgnore.current) {
                setIsLoading(false);
            }
        }
    }, [categoryId]);

    useEffect(() => {
        const signalIgnore = { current: false };

        fetchData(false, signalIgnore);

        return () => {
            signalIgnore.current = true;
        };
    }, [fetchData]);

    const refetch = useCallback(() => {
        return fetchData(true);
    }, [fetchData]);

    const isCached = !!getValidCache(categoryId);

    return {
        data,
        isLoading,
        error,
        refetch,
        isCached
    };
};