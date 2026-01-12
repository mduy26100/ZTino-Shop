import { useQuery } from '../../../hooks/utils';
import { getOrderDetail } from '../api';

const CACHE_TTL = 5 * 60 * 1000;

export const useGetOrderDetail = (orderCode, options = {}) => {
    return useQuery(
        orderCode ? `order-detail-${orderCode}` : null,
        ({ signal }) => getOrderDetail(orderCode, { signal }),
        {
            ttl: CACHE_TTL,
            enabled: !!orderCode,
            onSuccess: options.onSuccess,
            onError: options.onError,
        }
    );
};