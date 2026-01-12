import { getGuestCartId } from '../../utils';
import { invalidateMyCartCache } from '../../features/cart/hooks/useGetMyCart';
import { invalidateGuestCartCache } from '../../features/cart/hooks/useGetCartById';

export const invalidateCartCacheByAuth = (isAuthenticated, cartIdOverride = null) => {
    if (isAuthenticated) {
        invalidateMyCartCache();
    } else {
        const guestCartId = cartIdOverride || getGuestCartId();
        invalidateGuestCartCache(guestCartId);
    }
};
