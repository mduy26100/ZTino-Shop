import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getColorsByProductId = (productId) => {
    return axiosClient.get(`${ENDPOINTS.ADMIN.PRODUCT_COLORS}/${productId}`);
};