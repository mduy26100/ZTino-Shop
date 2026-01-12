import { useMutation } from '../../../../hooks/utils';
import { deleteProductColor } from '../../api';

export const useDeleteProductColor = () => {
    const { mutate, isLoading } = useMutation(deleteProductColor);
    return { remove: mutate, isDeleting: isLoading };
};