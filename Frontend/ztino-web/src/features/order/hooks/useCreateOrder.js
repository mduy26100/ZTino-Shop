import { useState, useCallback } from 'react';
import { useAuth } from '../../../contexts';
import { getGuestCartId } from '../../../utils';
import { invalidateMyCartCache } from '../../cart/hooks/useGetMyCart';
import { invalidateGuestCartCache } from '../../cart/hooks/useGetCartById';
import { createOrder } from '../api';

export const useCreateOrder = () => {
    const [isLoading, setIsLoading] = useState(false);
    const { isAuthenticated } = useAuth();

    const create = useCallback(async (orderData, options = {}) => {
        const { onSuccess, onError } = options;

        setIsLoading(true);

        try {
            const response = await createOrder(orderData);

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
        }
    }, [isAuthenticated]);

    return { create, isLoading };
};