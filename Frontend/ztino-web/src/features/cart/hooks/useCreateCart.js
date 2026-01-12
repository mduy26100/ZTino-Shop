import { useCallback } from 'react';
import { useMutation, invalidateCartCacheByAuth } from '../../../hooks/utils';
import { useAuth } from '../../../contexts';
import { getGuestCartId, setGuestCartId } from '../../../utils';
import { createCart } from '../api';

export const useCreateCart = () => {
    const { isAuthenticated } = useAuth();

    const { mutate, isLoading } = useMutation(
        async (cartData) => {
            const payload = {
                productVariantId: cartData.productVariantId,
                quantity: cartData.quantity,
            };

            if (!isAuthenticated) {
                const guestCartId = getGuestCartId();
                if (guestCartId) payload.cartId = guestCartId;
            }

            const response = await createCart(payload);

            if (!isAuthenticated && response?.cartId) {
                setGuestCartId(response.cartId);
            }
            invalidateCartCacheByAuth(isAuthenticated, response?.cartId);

            return response;
        }
    );

    return { create: mutate, isLoading };
};
