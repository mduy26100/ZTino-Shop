import { useMutation } from '../../../../hooks/utils';
import { createProductImages } from '../../api';

export const useCreateProductImages = () => {
    const { mutate, isLoading } = useMutation(createProductImages);
    return { create: mutate, isCreating: isLoading };
};