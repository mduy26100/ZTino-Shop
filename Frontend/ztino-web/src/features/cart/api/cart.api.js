import axiosClient from "../../../config/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const getMyCart = () => {
    return axiosClient.get(`${ENDPOINTS.CART}`);
};

export const getCartById = (id) => {
    return axiosClient.get(`${ENDPOINTS.CART}/${id}`);
};

export const createCart = (cart) => {
    const dto = {
        productVariantId: cart.productVariantId,
        quantity: cart.quantity,
    };

    // Only include cartId if provided (for guest cart or merge scenario)
    if (cart.cartId) {
        dto.cartId = cart.cartId;
    }

    return axiosClient.post(`${ENDPOINTS.CART}`, { dto });
};