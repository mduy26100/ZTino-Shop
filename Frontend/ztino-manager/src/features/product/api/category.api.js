import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getCategories = () => {
    return axiosClient.get(ENDPOINTS.CATEGORIES);
};

export const createCategory = (payload) => {
    const formData = new FormData();

    formData.append("Name", payload.name?.trim() || "");
    formData.append("Slug", payload.slug?.trim() || "");

    formData.append("IsActive", payload.isActive ?? true);

    if (payload.parentId) {
        formData.append("ParentId", payload.parentId);
    }

    if (payload.image) {
        formData.append("ImageUrl", payload.image); 
    }

    return axiosClient.post(ENDPOINTS.ADMIN.CATEGORIES, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const updateCategory = (payload) => {
    const formData = new FormData();

    formData.append("Id", payload.id);
    formData.append("Name", payload.name?.trim() || "");
    formData.append("Slug", payload.slug?.trim() || "");

    formData.append("IsActive", payload.isActive ?? true);

    if (payload.parentId) {
        formData.append("ParentId", payload.parentId);
    }

    if (payload.image) {
        formData.append("ImageUrl", payload.image); 
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.CATEGORIES}/${payload.id}`, formData, {
        headers: {
            "Content-Type": "multipart/form-data",
        },
    });
};

export const deleteCategory = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.CATEGORIES}/${id}`);
};
