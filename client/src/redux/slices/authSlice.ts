import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";

export interface AuthState {
  isLoggedIn: boolean;
  user?: {
    userId: string;
    username: string;
  };
}

const initialState: AuthState = {
  isLoggedIn: false,
};

export const authSlice = createSlice({
  name: "auth",
  initialState,
  reducers: {
    setLoggedIn: (state, action: PayloadAction<boolean>) => {
      state.isLoggedIn = action.payload;
    },
    setUser: (
      state,
      action: PayloadAction<{ userId: string; username: string } | undefined>
    ) => {
      state.user = action.payload;
    },
  },
  selectors: {
    getIsLoggedIn: (state: AuthState) => state.isLoggedIn,
    getUser: (state: AuthState) => state.user,
  },
});

export const { setLoggedIn, setUser } = authSlice.actions;
export const { getIsLoggedIn, getUser } = authSlice.selectors;

export default authSlice.reducer;
