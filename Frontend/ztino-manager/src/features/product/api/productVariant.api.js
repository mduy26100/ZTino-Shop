import { ENDPOINTS } from "../../../constants";
import { axiosClient } from "../../../services";

export const createProductVariant = (payload) => {
    const dto = {
        productColorId: payload.productColorId,
        sizeId: payload.sizeId,
        stockQuantity: payload.stockQuantity,
        price: payload.price,
        isActive: payload.isActive
    }

    return axiosClient.post(ENDPOINTS.ADMIN.PRODUCT_VARIANTS, {dto});
};

export const updateProductVariant = (payload) => {
    const dto = {
        id: payload.id,
        productColorId: payload.productColorId,
        sizeId: payload.sizeId,
        stockQuantity: payload.stockQuantity,
        price: payload.price,
        isActive: payload.isActive
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.PRODUCT_VARIANTS}/${payload.id}`, {dto});
};

export const deleteProductVariant = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.PRODUCT_VARIANTS}/${id}`);
};