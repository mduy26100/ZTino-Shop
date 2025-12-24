import axios from "axios";
import { clearAuth } from "../utils/localStorage";

const API_URL = import.meta.env.VITE_API_URL;

const axiosClient = axios.create({
  baseURL: API_URL,
  timeout: 30000,
  headers: {
    "Content-Type": "application/json",
    "Accept": "application/json",
  },
});

axiosClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
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
    if (error.code === 'ECONNABORTED') {
      return Promise.reject({
        Error: {
          Message: "Connection timed out. Please check your network or try again later.",
        },
        isTimeout: true,
      });
    }

    if (error.response?.status === 401) {
      if (window.location.pathname !== '/login') {
        clearAuth();
        window.location.href = '/login';
        return new Promise(() => {}); 
      }
    }

    if (error.response?.status === 403) {
      return Promise.reject({
        Error: {
          Message: "You do not have permission to perform this action.",
        },
      });
    }

    if (error.response?.data) {
      return Promise.reject(error.response.data);
    }

    return Promise.reject({
      Error: {
        Message: "Unable to connect to the server. Please check your internet connection.",
      },
      isNetworkError: true,
    });
  }
);

export default axiosClient;