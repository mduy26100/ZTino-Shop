import { useMutation } from '../../../../hooks/utils';
import { deleteColor } from '../../api';

export const useDeleteColor = () => {
    const { mutate, isLoading } = useMutation(deleteColor);
    return { remove: mutate, isDeleting: isLoading };
};