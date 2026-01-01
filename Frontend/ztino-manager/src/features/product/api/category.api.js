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
    const dto = {
        id: payload.id,
        name: payload.name,
        slug: payload.slug,
        isActive: payload.isActive ?? true,
        parentId: payload.parentId || null,
    };

    return axiosClient.put(`${ENDPOINTS.ADMIN.CATEGORIES}/${payload.id}`, { dto });
};

export const deleteCategory = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.CATEGORIES}/${id}`);
};
