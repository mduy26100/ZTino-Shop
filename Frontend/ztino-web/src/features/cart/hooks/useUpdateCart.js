import { useMutation, invalidateCartCacheByAuth } from '../../../hooks/utils';
import { useAuth } from '../../../contexts';
import { updateCart } from '../api';

export const useUpdateCart = () => {
    const { isAuthenticated } = useAuth();

    const { mutate, isLoading, mutatingVariables } = useMutation(
        async (updateData) => {
            if (!updateData.cartId) {
                throw new Error('cartId is required for update');
            }

            const payload = {
                cartId: updateData.cartId,
                productVariantId: updateData.productVariantId,
                quantity: updateData.quantity,
            };

            const response = await updateCart(payload);
            invalidateCartCacheByAuth(isAuthenticated);
            return response;
        }
    );

    return {
        update: mutate,
        isLoading,
        updatingItemId: mutatingVariables?.cartItemId || null,
    };
};