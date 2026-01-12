import { useMutation } from '../../../../hooks/utils';
import { updateSize } from '../../api';

export const useUpdateSize = () => {
    const { mutate, isLoading } = useMutation(updateSize);
    return { update: mutate, isUpdating: isLoading };
};