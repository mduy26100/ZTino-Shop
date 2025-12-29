import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ProductImagesManager";

export const getProductImagesByProductVariantId = (variantId) => {
    return axiosClient.get(`${URL_MANAGER}/${variantId}/images`);
};