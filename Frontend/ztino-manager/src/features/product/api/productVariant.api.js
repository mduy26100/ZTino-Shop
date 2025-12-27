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