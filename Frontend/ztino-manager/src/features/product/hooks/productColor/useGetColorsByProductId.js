import { useQuery } from '../../../../hooks/utils';
import { getColorsByProductId } from '../../api';

export const useGetColorsByProductId = (productId) => {
    return useQuery(
        productId ? `product-colors-${productId}` : null,
        ({ signal }) => getColorsByProductId(productId, { signal }),
        { enabled: !!productId }
    );
};