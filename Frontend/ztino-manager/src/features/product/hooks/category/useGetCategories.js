import { useQuery } from '../../../../hooks/utils';
import { getCategories } from '../../api';

export const useGetCategories = () => {
    return useQuery('categories', getCategories, { initialData: [] });
};