import { ENDPOINTS } from "../../../constants";
import { axiosClient } from "../../../config";

export const getColors = () => {
    return axiosClient.get(ENDPOINTS.COLORS);
};

export const createColor = (payload) => {
    const dto = {
        name: payload.name
    }

    return axiosClient.post(ENDPOINTS.ADMIN.COLORS, {dto});
};

export const updateColor = (payload) => {
    const dto = {
        id: payload.id,
        name: payload.name
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.COLORS}/${payload.id}`, {dto});
};

export const deleteColor = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.COLORS}/${id}`);
};