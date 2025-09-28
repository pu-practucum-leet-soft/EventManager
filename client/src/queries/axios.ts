import axios from "axios";
import config from "@config";
import { clearCredentials, setCredentials } from "@redux/slices/authSlice";
import { store } from "@redux/store";

const instance = axios.create({
  baseURL: config.apiBaseUrl,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

// Request interceptor - add token to headers
instance.interceptors.request.use(
  (config) => {
    const state = store.getState();
    const token = state.auth.accessToken;

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Response interceptor - handle 401 and refresh
instance.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        // This instance doesn't have interceptors, preventing infinite loops
        const refreshAxios = axios.create({
          baseURL: config.apiBaseUrl,
          withCredentials: true,
          headers: {
            "Content-Type": "application/json",
          },
        });

        const response = await refreshAxios.post("/users/refresh", {});
        const newToken = response.data.token;

        if (!newToken) {
          store.dispatch(clearCredentials());
          return Promise.reject(error);
        }

        store.dispatch(
          setCredentials({ accessToken: newToken, user: response.data.user })
        );

        originalRequest.headers.Authorization = `Bearer ${newToken}`;
        return instance(originalRequest);
      } catch (err) {
        store.dispatch(clearCredentials());
        return Promise.reject(err);
      }
    }

    return Promise.reject(error);
  }
);

export default instance;
