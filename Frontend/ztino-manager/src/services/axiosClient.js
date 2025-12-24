import axios from "axios";
import { clearAuth } from "../utils/localStorage";

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
    if (response.status === 204) {
      return true; 
    }

    const res = response.data;

    if (res && typeof res.StatusCode === 'number') {
        if (res.StatusCode >= 200 && res.StatusCode < 300) {
            return res.Data;
        }
        return Promise.reject(res);
    }

    if (response.status >= 200 && response.status < 300) {
        return res;
    }

    return Promise.reject(res);
  },
  (error) => {
    if (error.response?.status === 401) {
      if (window.location.pathname !== '/login') {
        clearAuth(); 
        window.location.href = '/login';
        return Promise.reject(error);
      }
    }

    if (error.response?.data) {
      return Promise.reject(error.response.data);
    }

    return Promise.reject({
      Error: {
        Message: "Network error. Please check your connection.",
      },
    });
  }
);

export default axiosClient;