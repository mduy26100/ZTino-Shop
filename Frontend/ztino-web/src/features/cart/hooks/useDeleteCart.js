import { useMutation, invalidateCartCacheByAuth } from '../../../hooks/utils';
import { useAuth } from '../../../contexts';
import { deleteCart } from '../api';

export const useDeleteCart = () => {
    const { isAuthenticated } = useAuth();

    const { mutate, isLoading, mutatingVariables } = useMutation(
        async (cartItemId) => {
            if (!cartItemId) {
                throw new Error('cartItemId is required for delete');
            }

            const response = await deleteCart(cartItemId);
            invalidateCartCacheByAuth(isAuthenticated);
            return response;
        }
    );

    return {
        remove: mutate,
        isLoading,
        deletingItemId: mutatingVariables,
    };
};