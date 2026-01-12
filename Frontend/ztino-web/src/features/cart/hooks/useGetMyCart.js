import { useQuery, invalidateCache } from '../../../hooks/utils';
import { getMyCart } from '../api/cart.api';

const CACHE_KEY = 'my-cart';
const CACHE_TTL = 0;

export const invalidateMyCartCache = () => {
    invalidateCache(CACHE_KEY);
};

export const useGetMyCart = (options = {}) => {
    const { enabled = true } = options;
    
    return useQuery(CACHE_KEY, getMyCart, {
        ttl: CACHE_TTL,
        enabled,
        initialData: null,
    });
};