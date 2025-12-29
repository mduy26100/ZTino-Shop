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

export const updateProductImage = (productImagesData) => {
    const formData = new FormData();

    formData.append("Id", productImagesData.Id);
    formData.append("ProductVariantId", productImagesData.ProductVariantId);
    formData.append("IsMain", productImagesData.IsMain);

    
    if (productImagesData.ImageFile) {
        formData.append("ImageFile", productImagesData.ImageFile); 
    }

    return axiosClient.put(`${URL_MANAGER}/${productImagesData.Id}`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const deleteProductImage = (id) => {
    return axiosClient.delete(`${URL_MANAGER}/${id}`);
};