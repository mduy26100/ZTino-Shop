import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getCategories = () => {
    return axiosClient.get(ENDPOINTS.CATEGORIES);
};

export const createCategory = (payload) => {
    const dto = {
        name: payload.name,
        slug: payload.slug,
        isActive: payload.isActive ?? true,
        parentId: payload.parentId || null,
    };

    return axiosClient.post(ENDPOINTS.ADMIN.CATEGORIES, { dto });
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
