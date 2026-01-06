import { ENDPOINTS } from "../../../constants/apiEndpoints";
import axiosClient from "../../../config/axiosClient";

export const loginAPI = async (payload) => {
    const dto = {
        identifier: payload.identifier,
        password: payload.password
    }

    return await axiosClient.post(`${ENDPOINTS.AUTH_LOGIN}`, {dto});
}

export const registerAPI = async (payload) => {
    const dto = {
        firstName: payload.firstName,
        lastName: payload.lastName,
        userName: payload.userName,
        phoneNumber: payload.phoneNumber,
        password: payload.password,
        confirmPassword: payload.confirmPassword
    }

    return await axiosClient.post(`${ENDPOINTS.AUTH_REGISTER}`, {dto});
}