import { useMutation } from '../../../../hooks/utils';
import { deleteCategory } from '../../api';

export const useDeleteCategory = () => {
    const { mutate, isLoading } = useMutation(deleteCategory);
    return { remove: mutate, isDeleting: isLoading };
};