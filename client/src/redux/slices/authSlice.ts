import { createSlice } from "@reduxjs/toolkit";
import type { PayloadAction } from "@reduxjs/toolkit";

export interface AuthState {
  isLoggedIn: boolean;
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
  },
  selectors: {
    getIsLoggedIn: (state: AuthState) => state.isLoggedIn,
  },
});

export const { setLoggedIn } = authSlice.actions;
export const { getIsLoggedIn } = authSlice.selectors;

export default authSlice.reducer;
