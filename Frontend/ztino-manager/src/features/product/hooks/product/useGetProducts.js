import { useQuery } from '../../../../hooks/utils';
import { getProducts } from '../../api';

export const useGetProducts = () => {
    return useQuery('products', getProducts, { initialData: [] });
};