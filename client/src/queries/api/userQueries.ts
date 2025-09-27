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
};

export default userQueries;
