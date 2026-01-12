import { useMutation } from '../../../hooks/utils';
import { updateMyOrderStatus } from '../api';

export const useUpdateMyOrderStatus = () => {
    const { mutate, isLoading } = useMutation(updateMyOrderStatus);
    return { updateStatus: mutate, isUpdating: isLoading };
};
