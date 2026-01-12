import { useQuery } from '../../../hooks/utils';
import { getMyOrders } from '../api';

const CACHE_KEY = 'my-orders';
const CACHE_TTL = 5 * 60 * 1000;

export const useGetMyOrders = () => {
    return useQuery(CACHE_KEY, getMyOrders, {
        ttl: CACHE_TTL,
        initialData: [],
        transformResponse: (res) => {
            const rawData = res?.data || res;
            return Array.isArray(rawData) ? rawData : [];
        },
    });
};