import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getProductsByCategoryId = (categoryId) => {
    return axiosClient.get(`${ENDPOINTS.PRODUCTS}/category/${categoryId}`);
};

export const getProductDetailBySlug = (slug) => {
    return axiosClient.get(`${ENDPOINTS.PRODUCTS}/${slug}`);
};
