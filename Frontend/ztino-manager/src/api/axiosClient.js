import axios from "axios";

const API_URL = import.meta.env.VITE_API_URL;

const axiosClient = axios.create({
  baseURL: API_URL,
});

axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

axiosClient.interceptors.response.use(
  (response) => {
    const res = response.data;

    if (res?.StatusCode >= 200 && res?.StatusCode < 300) {
      return res.Data;
    }

    return Promise.reject(res);
  },
  (error) => {
    if (error.response?.data) {
      return Promise.reject(error.response.data);
    }

    return Promise.reject({
      Error: {
        Message: "Network error",
      },
    });
  }
);

export default axiosClient;
