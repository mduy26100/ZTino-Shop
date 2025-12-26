import axiosClient from "../../../services/axiosClient";

const URL_MANAGER = "ProductsManager";
const URL_BASE = "Products";

export const getProducts = () => {
    return axiosClient.get(URL_BASE);
};