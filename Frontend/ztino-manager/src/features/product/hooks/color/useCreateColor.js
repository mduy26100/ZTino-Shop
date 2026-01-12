import { useMutation } from '../../../../hooks/utils';
import { createColor } from '../../api';

export const useCreateColor = () => {
    const { mutate, isLoading } = useMutation(createColor);
    return { create: mutate, isCreating: isLoading };
};