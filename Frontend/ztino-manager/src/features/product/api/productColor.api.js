import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

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