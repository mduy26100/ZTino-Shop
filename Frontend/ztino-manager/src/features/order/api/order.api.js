import axiosClient from "../../../config/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const getAllOrders = () => {
    return axiosClient.get(ENDPOINTS.ADMIN.ORDER)
}

export const getOrderDetail = (orderCode) => {
    return axiosClient.get(`${ENDPOINTS.ADMIN.ORDER}/${orderCode}`);
};

export const updateOrderStatus = (payload) => {
    const dto = {
        orderId: payload.orderId,
        newStatus: payload.newStatus,
        note: payload.note,
        cancelReason: payload.cancelReason
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.ORDER}/${payload.orderId}`, {dto});
};