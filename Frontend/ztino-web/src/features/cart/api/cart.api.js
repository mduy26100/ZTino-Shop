import axiosClient from "../../../services/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const getMyCart = () => {
    return axiosClient.get(`${ENDPOINTS.CART}`);
};

export const getCartById = (id) => {
    return axiosClient.get(`${ENDPOINTS.CART}/${id}`);
};