import { useQuery } from '../../../../hooks/utils';
import { getProductsByCategoryId } from '../../api/product.api';

const CACHE_TTL = 5 * 60 * 1000;

export const useGetProductsByCategoryId = (categoryId) => {
    return useQuery(
        categoryId ? `products-category-${categoryId}` : null,
        ({ signal }) => getProductsByCategoryId(categoryId, { signal }),
        {
            ttl: CACHE_TTL,
            enabled: !!categoryId,
            initialData: [],
            transformResponse: (res) => {
                const rawData = res?.data || res;
                return Array.isArray(rawData) ? rawData : [];
            },
        }
    );
};