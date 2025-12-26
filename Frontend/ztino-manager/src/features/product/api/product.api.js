import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ProductsManager";
const URL_BASE = "Products";

export const getProducts = () => {
    return axiosClient.get(URL_BASE);
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

    return axiosClient.post(URL_MANAGER, formData, {
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

    return axiosClient.put(`${URL_MANAGER}/${productData.Id}`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};