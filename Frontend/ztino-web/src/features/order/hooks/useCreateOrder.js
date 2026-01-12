import { useMutation, invalidateCartCacheByAuth } from '../../../hooks/utils';
import { useAuth } from '../../../contexts';
import { createOrder } from '../api';

export const useCreateOrder = () => {
    const { isAuthenticated } = useAuth();

    const { mutate, isLoading } = useMutation(
        async (orderData) => {
            const response = await createOrder(orderData);
            invalidateCartCacheByAuth(isAuthenticated);
            return response;
        }
    );

    return { create: mutate, isLoading };
};