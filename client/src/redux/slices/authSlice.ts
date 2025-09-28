import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";

export interface AuthState {
  accessToken: string | null;
  user: {
    userId: string;
    username: string;
  } | null;
}

const initialState: AuthState = {
  accessToken: null,
  user: null,
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setCredentials: (
      state,
      action: PayloadAction<{
        accessToken: string;
        user: { userId: string; username: string };
      }>
    ) => {
      state.accessToken = action.payload.accessToken;
      state.user = action.payload.user;
    },
    clearCredentials: (state) => {
      state.accessToken = null;
      state.user = null;
    },
  },
  selectors: {
    getUser: (state: AuthState) => state.user,
  },
});

export const { setCredentials, clearCredentials } = authSlice.actions;
export const { getUser } = authSlice.selectors;

export default authSlice.reducer;
