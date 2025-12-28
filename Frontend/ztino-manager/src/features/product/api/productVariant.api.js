import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ProductVariantsManager";

export const createProductVariant = (payload) => {
    const dto = {
        productId: payload.productId,
        colorId: payload.colorId,
        sizeId: payload.sizeId,
        stockQuantity: payload.stockQuantity,
        price: payload.price,
        isActive: payload.isActive
    }

    return axiosClient.post(URL_MANAGER, {dto});
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

    return axiosClient.put(`${URL_MANAGER}/${payload.id}`, {dto});
};

export const deleteProductVariant = (id) => {
    return axiosClient.delete(`${URL_MANAGER}/${id}`);
};