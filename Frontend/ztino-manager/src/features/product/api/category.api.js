import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "CategoriesManager";
const URL_BASE = "Categories";

export const getCategories = () => {
    return axiosClient.get(URL_BASE);
};