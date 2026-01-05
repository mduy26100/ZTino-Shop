import { axiosClient } from "../../../services";
import { ENDPOINTS } from "../../../constants";

export const getSizes = () => {
    return axiosClient.get(ENDPOINTS.SIZES);
};

export const createSize = (payload) => {
    const dto = {
        name: payload.name
    }

    return axiosClient.post(ENDPOINTS.ADMIN.SIZES, {dto});
};

export const updateSize = (payload) => {
    const dto = {
        id: payload.id,
        name: payload.name
    }

    return axiosClient.put(`${ENDPOINTS.ADMIN.SIZES}/${payload.id}`, {dto});
};

export const deleteSize = (id) => {
    return axiosClient.delete(`${ENDPOINTS.ADMIN.SIZES}/${id}`);
};