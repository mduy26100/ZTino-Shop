import { useState, useCallback } from 'react';
import { useAuth } from '../../../contexts';
import { getGuestCartId } from '../../../utils';
import { invalidateMyCartCache } from './useGetMyCart';
import { invalidateGuestCartCache } from './useGetCartById';
import { deleteCart } from '../api';

export const useDeleteCart = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [deletingItemId, setDeletingItemId] = useState(null);
    const { isAuthenticated } = useAuth();

    const remove = useCallback(async (cartItemId, options = {}) => {
        const { onSuccess, onError } = options;

        if (!cartItemId) {
            const error = new Error('cartItemId is required for delete');
            onError?.(error);
            throw error;
        }

        setIsLoading(true);
        setDeletingItemId(cartItemId);

        try {
            const response = await deleteCart(cartItemId);

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
            setDeletingItemId(null);
        }
    }, [isAuthenticated]);

    return { remove, isLoading, deletingItemId };
};