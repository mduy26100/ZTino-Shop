import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../services/axiosClient";

const loginAPI = async (data) => {
    try {
        const response = await axiosClient.post(`${ENDPOINTS.AUTH}/login`, {
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

export {
    loginAPI
}