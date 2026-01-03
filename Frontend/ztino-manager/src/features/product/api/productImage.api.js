import axiosClient from "../../../services/axiosClient";
import { ENDPOINTS } from "../../../constants/apiEndpoints";

export const getProductImagesByProductColorId = (productColorId) => {
    return axiosClient.get(ENDPOINTS.ADMIN.PRODUCT_IMAGES, {
        params: { productColorId: productColorId } 
    });
};

export const createProductImages = (productImagesData) => {
    const formData = new FormData();

    formData.append("ProductColorId", productImagesData.ProductColorId);

    if (productImagesData.ImageFiles?.length) {
        productImagesData.ImageFiles.forEach((file) => {
            formData.append("ImageFiles", file);
        });
    }

    return axiosClient.post(ENDPOINTS.ADMIN.PRODUCT_IMAGES, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const updateProductImage = (productImagesData) => {
    const formData = new FormData();

    formData.append("Id", productImagesData.Id);
    formData.append("ProductColorId", productImagesData.ProductColorId);
    formData.append("IsMain", productImagesData.IsMain);

    
    if (productImagesData.ImageFile) {
        formData.append("ImageFile", productImagesData.ImageFile); 
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.PRODUCT_IMAGES}/${productImagesData.Id}`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const deleteProductImage = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.PRODUCT_IMAGES}/${id}`);
};