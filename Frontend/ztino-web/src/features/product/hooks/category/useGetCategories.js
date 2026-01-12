import { useQuery } from '../../../../hooks/utils';
import { getCategories } from '../../api/category.api';

const CACHE_KEY = 'categories';
const CACHE_TTL = 5 * 60 * 1000;

export const useGetCategories = () => {
    return useQuery(CACHE_KEY, getCategories, {
        ttl: CACHE_TTL,
        initialData: [],
        transformResponse: (res) => {
            const rawData = res?.data || res;
            return Array.isArray(rawData) ? rawData : [];
        },
    });
};