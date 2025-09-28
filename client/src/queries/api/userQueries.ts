import axios from "@queries/axios";
import { Token } from "@queries/types/user.types";

type LoginResponse = {
  token: Token;
  message: string;
  statusCode: number; // TODO: Business Status code enum ? Although is it even needed in the client ?
};

const BASE_URL = `/users`;

const userQueries = {
  login: async (data: { email: string; password: string }) => {
    const response = await axios.post<LoginResponse>(`${BASE_URL}/login`, data);

    return response;
  },
  logout: async () => {
    const response = await axios.post(`${BASE_URL}/logout`, {});
    return response;
  },
  refresh: async () => {
    const response = await axios.post<{ token: string }>(
      `${BASE_URL}/refresh`,
      {
        withCredentials: true,
      }
    );

    return response;
  },
};

export default userQueries;
