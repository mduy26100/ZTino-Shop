import { useQuery } from '../../../hooks/utils';
import { getAllOrders } from '../api';

export const useGetAllOrders = () => {
    return useQuery('orders', getAllOrders, { initialData: [] });
};