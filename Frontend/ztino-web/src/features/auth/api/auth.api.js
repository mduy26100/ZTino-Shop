import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

export const loginAPI = async (data) => {
    try {
        const response = await axiosClient.post(`${ENDPOINTS.AUTH_LOGIN}`, {
            dto: {
                identifier: data.identifier,
                password: data.password
            }
        });
        return response;
    } catch (error) {
        throw error;
    }
}