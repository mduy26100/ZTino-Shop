import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "SizesManager";
const URL_BASE = "Sizes";

export const getSizes = () => {
    return axiosClient.get(URL_BASE);
};