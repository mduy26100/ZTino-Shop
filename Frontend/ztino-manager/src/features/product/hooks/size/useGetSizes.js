import { useQuery } from '../../../../hooks/utils';
import { getSizes } from '../../api';

export const useGetSizes = () => {
    return useQuery('sizes', getSizes, { initialData: [] });
};