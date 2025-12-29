import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const createProductVariant = (payload) => {
    const dto = {
        productId: payload.productId,
        colorId: payload.colorId,
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
        productId: payload.productId,
        colorId: payload.colorId,
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