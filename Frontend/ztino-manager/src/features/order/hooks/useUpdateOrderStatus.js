import { useMutation } from '../../../hooks/utils';
import { updateOrderStatus } from '../api';

export const useUpdateOrderStatus = () => {
    const { mutate, isLoading } = useMutation(updateOrderStatus);
    return { updateStatus: mutate, isUpdating: isLoading };
};
