import axios from "axios";
import { clearAuth } from "../utils";

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
      return null;
    }

    const apiResponse = response.data;

    if (apiResponse && apiResponse.error) {
      return Promise.reject(apiResponse);
    }

    return apiResponse.data;
  },
  (error) => {
    if (error.code === 'ECONNABORTED') {
      return Promise.reject({
        error: {
          type: 'timeout',
          message: "Connection timed out. Please check your network or try again later.",
          details: null
        },
        meta: { timestamp: new Date().toISOString() } 
      });
    }

    if (error.response) {
      const apiResponse = error.response.data;

      if (error.response.status === 401) {
        if (window.location.pathname !== '/login') {
          clearAuth();
          window.location.href = '/login';
          return new Promise(() => {}); 
        }
      }

      return Promise.reject(apiResponse);
    }

    return Promise.reject({
      error: {
        type: 'network-error',
        message: "Unable to connect to the server. Please check your internet connection.",
        details: null
      },
      meta: { timestamp: new Date().toISOString() }
    });
  }
);

export default axiosClient;