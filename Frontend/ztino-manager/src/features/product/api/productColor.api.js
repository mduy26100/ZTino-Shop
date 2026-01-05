import { ENDPOINTS } from "../../../constants";
import { axiosClient } from "../../../services";

export const getColorsByProductId = (productId) => {
    return axiosClient.get(`${ENDPOINTS.ADMIN.PRODUCT_COLORS}/${productId}`);
};

export const createProductColor = (payload) => {
    const dto = {
        productId: payload.productId,
        colorId: payload.colorId
    }

    return axiosClient.post(ENDPOINTS.ADMIN.PRODUCT_COLORS, {dto});
};

export const deleteProductColor = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.PRODUCT_COLORS}/${id}`);
};