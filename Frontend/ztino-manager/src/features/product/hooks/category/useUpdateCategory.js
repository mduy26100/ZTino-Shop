import { useMutation } from '../../../../hooks/utils';
import { updateCategory } from '../../api';

export const useUpdateCategory = () => {
    const { mutate, isLoading } = useMutation(updateCategory);
    return { update: mutate, isUpdating: isLoading };
};