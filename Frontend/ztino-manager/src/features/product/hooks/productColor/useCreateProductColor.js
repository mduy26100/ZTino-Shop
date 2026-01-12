import { useMutation } from '../../../../hooks/utils';
import { createProductColor } from '../../api';

export const useCreateProductColor = () => {
    const { mutate, isLoading } = useMutation(createProductColor);
    return { create: mutate, isCreating: isLoading };
};