import { useState, useCallback } from 'react';
import { useAuth } from '../../../contexts';
import { getGuestCartId } from '../../../utils';
import { invalidateMyCartCache } from './useGetMyCart';
import { invalidateGuestCartCache } from './useGetCartById';
import { updateCart } from '../api';

export const useUpdateCart = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [updatingItemId, setUpdatingItemId] = useState(null);
    const { isAuthenticated } = useAuth();

    const update = useCallback(async (updateData, options = {}) => {
        const { onSuccess, onError } = options;

        if (!updateData.cartId) {
            const error = new Error('cartId is required for update');
            onError?.(error);
            throw error;
        }

        setIsLoading(true);
        setUpdatingItemId(updateData.cartItemId || null);

        try {
            const payload = {
                cartId: updateData.cartId,
                productVariantId: updateData.productVariantId,
                quantity: updateData.quantity,
            };

            const response = await updateCart(payload);

            if (isAuthenticated) {
                invalidateMyCartCache();
            } else {
                const guestCartId = getGuestCartId();
                invalidateGuestCartCache(guestCartId);
            }

            onSuccess?.(response);
            return response;
        } catch (error) {
            onError?.(error);
            throw error;
        } finally {
            setIsLoading(false);
            setUpdatingItemId(null);
        }
    }, [isAuthenticated]);

    return { update, isLoading, updatingItemId };
};