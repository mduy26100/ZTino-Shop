import { useQuery } from '../../../../hooks/utils';
import { getColors } from '../../api';

export const useGetColors = () => {
    return useQuery('colors', getColors, { initialData: [] });
};