import { useMutation } from '../../../../hooks/utils';
import { updateColor } from '../../api';

export const useUpdateColor = () => {
    const { mutate, isLoading } = useMutation(updateColor);
    return { update: mutate, isUpdating: isLoading };
};