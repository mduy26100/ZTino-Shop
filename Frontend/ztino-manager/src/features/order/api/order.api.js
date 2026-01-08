import axiosClient from "../../../config/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const getAllOrders = () => {
    return axiosClient.get(ENDPOINTS.ADMIN.ORDER)
}

export const getOrderDetail = (orderCode) => {
    return axiosClient.get(`${ENDPOINTS.ADMIN.ORDER}/${orderCode}`);
};