import { useMutation } from '../../../../hooks/utils';
import { createCategory } from '../../api';

export const useCreateCategory = () => {
    const { mutate, isLoading } = useMutation(createCategory);
    return { create: mutate, isLoading };
};