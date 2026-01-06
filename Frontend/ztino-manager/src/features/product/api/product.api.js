import { axiosClient } from "../../../config";
import { ENDPOINTS } from "../../../constants";

export const getProducts = () => {
    return axiosClient.get(ENDPOINTS.PRODUCTS);
};

export const getProductDetailById = (id) => {
    return axiosClient.get(`${ENDPOINTS.PRODUCTS}/${id}`);
};

export const createProduct = (productData) => {
    const formData = new FormData();

    formData.append("CategoryId", productData.CategoryId);
    formData.append("Name", productData.Name);
    formData.append("Slug", productData.Slug);
    formData.append("BasePrice", productData.BasePrice);
    formData.append("Description", productData.Description);
    formData.append("IsActive", productData.IsActive);
    formData.append("UpdatedAt", new Date().toISOString());
    
    if (productData.MainImageUrl) {
        formData.append("MainImageUrl", productData.MainImageUrl); 
    }

    return axiosClient.post(ENDPOINTS.ADMIN.PRODUCTS, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const updateProduct = (productData) => {
    const formData = new FormData();

    formData.append("Id", productData.Id);
    formData.append("CategoryId", productData.CategoryId);
    formData.append("Name", productData.Name);
    formData.append("Slug", productData.Slug);
    formData.append("BasePrice", productData.BasePrice);
    formData.append("Description", productData.Description);
    formData.append("IsActive", productData.IsActive);
    formData.append("UpdatedAt", new Date().toISOString());
    
    if (productData.MainImageUrl) {
        formData.append("MainImageUrl", productData.MainImageUrl); 
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.PRODUCTS}/${productData.Id}`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const deleteProduct = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.PRODUCTS}/${id}`);
};
