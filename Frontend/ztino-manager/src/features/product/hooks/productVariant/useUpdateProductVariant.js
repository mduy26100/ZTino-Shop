import { useMutation } from '../../../../hooks/utils';
import { updateProductVariant } from '../../api';

export const useUpdateProductVariant = () => {
    const { mutate, isLoading } = useMutation(updateProductVariant);
    return { update: mutate, isUpdating: isLoading };
};