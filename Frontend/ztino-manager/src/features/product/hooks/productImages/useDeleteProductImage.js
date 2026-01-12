import { useMutation } from '../../../../hooks/utils';
import { deleteProductImage } from '../../api';

export const useDeleteProductImage = () => {
    const { mutate, isLoading } = useMutation(deleteProductImage);
    return { remove: mutate, isDeleting: isLoading };
};