import { useMutation } from '../../../../hooks/utils';
import { createSize } from '../../api';

export const useCreateSize = () => {
    const { mutate, isLoading } = useMutation(createSize);
    return { create: mutate, isCreating: isLoading };
};