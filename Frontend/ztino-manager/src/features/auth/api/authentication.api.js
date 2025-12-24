import axiosClient from "../../../services/axiosClient";

const URL_BASE = "Authentications";

const loginAPI = async (data) => {
    try {
        const response = await axiosClient.post(`${URL_BASE}/login`, {
            dto: {
                email: data.email,
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