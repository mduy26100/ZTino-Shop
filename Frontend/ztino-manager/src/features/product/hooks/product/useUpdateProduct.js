import { useMutation } from '../../../../hooks/utils';
import { updateProduct } from '../../api';

export const useUpdateProduct = () => {
    const { mutate, isLoading } = useMutation(updateProduct);
    return { update: mutate, isUpdating: isLoading };
};