import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getProductsByCategoryId = (categoryId) => {
    return axiosClient.get(`${ENDPOINTS.PRODUCTS}/category/${categoryId}`);
};