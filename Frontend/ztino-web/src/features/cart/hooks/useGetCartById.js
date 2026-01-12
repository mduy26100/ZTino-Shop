import { useQuery, invalidateCache } from '../../../hooks/utils';
import { getCartById } from '../api/cart.api';

const CACHE_TTL = 0;

export const invalidateGuestCartCache = (cartId = null) => {
    if (cartId) {
        invalidateCache(`guest-cart-${cartId}`);
    }
};

export const useGetCartById = (id, options = {}) => {
    return useQuery(
        id ? `guest-cart-${id}` : null,
        ({ signal }) => getCartById(id, { signal }),
        {
            ttl: CACHE_TTL,
            enabled: !!id,
            onSuccess: options.onSuccess,
            onError: options.onError,
        }
    );
};