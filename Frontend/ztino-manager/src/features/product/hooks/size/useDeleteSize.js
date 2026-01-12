import { useMutation } from '../../../../hooks/utils';
import { deleteSize } from '../../api';

export const useDeleteSize = () => {
    const { mutate, isLoading } = useMutation(deleteSize);
    return { remove: mutate, isDeleting: isLoading };
};