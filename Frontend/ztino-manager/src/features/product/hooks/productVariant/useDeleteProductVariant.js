import { useMutation } from '../../../../hooks/utils';
import { deleteProductVariant } from '../../api';

export const useDeleteProductVariant = () => {
    const { mutate, isLoading } = useMutation(deleteProductVariant);
    return { remove: mutate, isDeleting: isLoading };
};