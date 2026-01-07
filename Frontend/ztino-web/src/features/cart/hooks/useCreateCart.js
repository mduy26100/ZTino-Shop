import { useState, useCallback } from 'react';
import { useAuth } from '../../../contexts';
import { 
    getGuestCartId, 
    setGuestCartId
} from '../../../utils';
import { invalidateMyCartCache } from './useGetMyCart';
import { invalidateGuestCartCache } from './useGetCartById';
import { createCart } from '../api';

export const useCreateCart = () => {
    const [isLoading, setIsLoading] = useState(false);
    const { isAuthenticated } = useAuth();

    const create = useCallback(async (cartData, options = {}) => {
        const { onSuccess, onError } = options;

        setIsLoading(true);
        try {
            const payload = {
                productVariantId: cartData.productVariantId,
                quantity: cartData.quantity,
            };

            if (!isAuthenticated) {
                const guestCartId = getGuestCartId();
                if (guestCartId) {
                    payload.cartId = guestCartId;
                }
            }

            const response = await createCart(payload);

            if (!isAuthenticated) {
                const returnedCartId = response?.cartId;
                if (returnedCartId) {
                    setGuestCartId(returnedCartId);
                }
                invalidateGuestCartCache(response?.cartId);
            } else {
                invalidateMyCartCache();
            }

            onSuccess?.(response);
            return response;
        } catch (error) {
            onError?.(error);
            throw error;
        } finally {
            setIsLoading(false);
        }
    }, [isAuthenticated]);

    return { create, isLoading };
};
