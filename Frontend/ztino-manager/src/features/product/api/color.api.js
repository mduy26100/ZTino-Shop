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