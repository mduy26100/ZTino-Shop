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