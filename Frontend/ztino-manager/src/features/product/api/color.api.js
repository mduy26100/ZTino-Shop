import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ColorsManager";
const URL_BASE = "Colors";

export const getColors = () => {
    return axiosClient.get(URL_BASE);
};

export const createColor = (payload) => {
    const dto = {
        name: payload.name
    }

    return axiosClient.post(URL_MANAGER, {dto});
};

export const updateColor = (payload) => {
    const dto = {
        id: payload.id,
        name: payload.name
    }

    return axiosClient.put(`${URL_MANAGER}/${payload.id}`, {dto});
};

export const deleteColor = (id) => {
    return axiosClient.delete(`${URL_MANAGER}/${id}`);
};