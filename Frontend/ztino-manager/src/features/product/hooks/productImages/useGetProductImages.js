import { useQuery } from '../../../../hooks/utils';
import { getProductImagesByProductColorId } from '../../api';

export const useGetProductImages = (productColorId) => {
    return useQuery(
        productColorId ? `product-images-${productColorId}` : null,
        ({ signal }) => getProductImagesByProductColorId(productColorId, { signal }),
        { 
            enabled: !!productColorId,
            initialData: [],
            transformResponse: (response) => {
                const rawData = response?.data || response;
                return Array.isArray(rawData) ? rawData : (rawData?.images || []);
            }
        }
    );
};