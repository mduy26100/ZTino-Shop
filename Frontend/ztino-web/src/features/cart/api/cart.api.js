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

    if (cart.cartId) {
        dto.cartId = cart.cartId;
    }

    return axiosClient.post(`${ENDPOINTS.CART}`, { dto });
};

export const updateCart = (cart) => {
    const dto = {
        cartId: cart.cartId,
        productVariantId: cart.productVariantId,
        quantity: cart.quantity,
    };

    return axiosClient.put(`${ENDPOINTS.CART}/${cart.cartId}`, { dto });
};

export const deleteCart = (cartItemId) => {
    return axiosClient.delete(`${ENDPOINTS.CART}/${cartItemId}`);
};