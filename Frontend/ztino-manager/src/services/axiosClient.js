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

    if (res && res.Error) {
      return Promise.reject(res);
    }

    if (res && res.Data !== undefined) {
      return res.Data;
    }

    return res;
  },
  (error) => {
    if (error.code === 'ECONNABORTED') {
      return Promise.reject({
        Error: {
          Type: 'timeout',
          Message: "Connection timed out. Please check your network or try again later.",
          Details: null
        },
        isTimeout: true,
      });
    }

    const serverError = error.response?.data;

    if (error.response?.status === 401) {
      if (window.location.pathname !== '/login') {
        clearAuth();
        window.location.href = '/login';
        return new Promise(() => {}); 
      }
      if (serverError) return Promise.reject(serverError);
    }

    if (error.response?.status === 403) {
      if (serverError) return Promise.reject(serverError);

      return Promise.reject({
        Error: {
          Type: 'forbidden',
          Message: "You do not have permission to perform this action.",
          Details: null
        },
      });
    }

    if (serverError) {
      return Promise.reject(serverError);
    }

    return Promise.reject({
      Error: {
        Type: 'network-error',
        Message: "Unable to connect to the server. Please check your internet connection.",
        Details: null
      },
      isNetworkError: true,
    });
  }
);

export default axiosClient;