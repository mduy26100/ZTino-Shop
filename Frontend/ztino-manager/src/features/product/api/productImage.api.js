import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ProductImagesManager";

export const getProductImagesByProductVariantId = (variantId) => {
    return axiosClient.get(`${URL_MANAGER}/${variantId}/images`);
};

export const createProductImages = (productImagesData) => {
    const formData = new FormData();

    formData.append("ProductVariantId", productImagesData.ProductVariantId);

    if (productImagesData.ImageFiles?.length) {
        productImagesData.ImageFiles.forEach((file) => {
            formData.append("ImageFiles", file);
        });
    }

    return axiosClient.post(URL_MANAGER, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};
