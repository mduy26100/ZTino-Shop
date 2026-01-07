import axiosClient from "../../../config/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const createOrder = (payload) => {
    const dto = {
        cartId: payload.cartId,
        selectedCartItemIds: payload.selectedCartItemIds,
        customerName: payload.customerName,
        customerPhone: payload.customerPhone,
        customerEmail: payload.customerEmail,
        shippingAddress: {
            recipientName: payload.shippingAddress.recipientName,
            phoneNumber: payload.shippingAddress.phoneNumber,
            street: payload.shippingAddress.street,
            ward: payload.shippingAddress.ward,
            district: payload.shippingAddress.district,
            city: payload.shippingAddress.city
        },
        note: payload.note
    }

    return axiosClient.post(`${ENDPOINTS.ORDER}`, {dto});
};