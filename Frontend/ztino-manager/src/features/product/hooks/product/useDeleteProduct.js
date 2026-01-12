import { useMutation } from '../../../../hooks/utils';
import { deleteProduct } from '../../api';

export const useDeleteProduct = () => {
    const { mutate, isLoading } = useMutation(deleteProduct);
    return { remove: mutate, isDeleting: isLoading };
};