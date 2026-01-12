import { useMutation } from '../../../../hooks/utils';
import { updateProductImage } from '../../api';

export const useUpdateProductImage = () => {
    const { mutate, isLoading } = useMutation(updateProductImage);
    return { update: mutate, isUpdating: isLoading };
};