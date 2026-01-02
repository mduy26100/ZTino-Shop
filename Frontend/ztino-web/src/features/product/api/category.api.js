import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const getCategories = () => {
    return axiosClient.get(ENDPOINTS.CATEGORIES);
};