import { useMutation } from '../../../../hooks/utils';
import { createProductVariant } from '../../api';

export const useCreateProductVariant = () => {
    const { mutate, isLoading } = useMutation(createProductVariant);
    return { create: mutate, isCreating: isLoading };
};