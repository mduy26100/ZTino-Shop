import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../config/axiosClient";

export const getCategories = () => {
    return axiosClient.get(ENDPOINTS.CATEGORIES);
};