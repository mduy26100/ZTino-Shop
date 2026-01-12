import { useQuery } from '../../../../hooks/utils';
import { getProductDetailById } from '../../api';

export const useGetProductDetailById = (id) => {
    return useQuery(
        id ? `product-${id}` : null,
        ({ signal }) => getProductDetailById(id, { signal }),
        { enabled: !!id }
    );
};