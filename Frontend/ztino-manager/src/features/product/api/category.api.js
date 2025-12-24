import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "CategoriesManager";
const URL_BASE = "Categories";

export const getCategories = () => {
    return axiosClient.get(URL_BASE);
};

export const createCategory = (payload) => {
    const dto = {
        name: payload.name,
        slug: payload.slug,
        isActive: payload.isActive ?? true,
        parentId: payload.parentId || null,
    };

    return axiosClient.post(URL_MANAGER, { dto });
};

export const updateCategory = (payload) => {
    const dto = {
        id: payload.id,
        name: payload.name,
        slug: payload.slug,
        isActive: payload.isActive ?? true,
        parentId: payload.parentId || null,
    };

    return axiosClient.put(`${URL_MANAGER}/${payload.id}`, { dto });
};

export const deleteCategory = (id) => {
    return axiosClient.delete(`${URL_MANAGER}/${id}`);
};
