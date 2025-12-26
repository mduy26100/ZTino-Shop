import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "SizesManager";
const URL_BASE = "Sizes";

export const getSizes = () => {
    return axiosClient.get(URL_BASE);
};

export const createSize = (payload) => {
    const dto = {
        name: payload.name
    }

    return axiosClient.post(URL_MANAGER, {dto});
};

export const updateSize = (payload) => {
    const dto = {
        id: payload.id,
        name: payload.name
    }

    return axiosClient.put(`${URL_MANAGER}/${payload.id}`, {dto});
};

export const deleteSize = (id) => {
    return axiosClient.delete(`${URL_MANAGER}/${id}`);
};