import { useMutation } from '../../../../hooks/utils';
import { createProduct } from '../../api';

export const useCreateProduct = () => {
    const { mutate, isLoading } = useMutation(createProduct);
    return { create: mutate, isCreating: isLoading };
};