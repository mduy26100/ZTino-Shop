import { useQuery } from '../../../hooks/utils';
import { getOrderDetail } from '../api';

export const useGetOrderDetail = (orderCode) => {
    return useQuery(
        orderCode ? `order-${orderCode}` : null,
        () => getOrderDetail(orderCode),
        { enabled: !!orderCode }
    );
};