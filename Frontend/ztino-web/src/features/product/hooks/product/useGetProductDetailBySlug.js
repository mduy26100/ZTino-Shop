import { useQuery } from '../../../../hooks/utils';
import { getProductDetailBySlug } from '../../api/product.api';

const CACHE_TTL = 5 * 60 * 1000;

export const useGetProductDetailBySlug = (slug, options = {}) => {
    return useQuery(
        slug ? `product-${slug}` : null,
        ({ signal }) => getProductDetailBySlug(slug, { signal }),
        {
            ttl: CACHE_TTL,
            enabled: !!slug,
            onSuccess: options.onSuccess,
            onError: options.onError,
        }
    );
};