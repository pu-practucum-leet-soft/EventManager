import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface ModalState {
  type: "addEvent" | "editEvent" | "inviteToEvent" | null;
  props: any;
}

const initialState: ModalState = {
  type: null,
  props: {},
};

export const modalSlice = createSlice({
  name: "modal",
  initialState,
  reducers: {
    openAddEventModal: (state, action: PayloadAction<any>) => {
      state.type = "addEvent";
      state.props = action.payload;
    },
    openInviteToEventModal: (state, action: PayloadAction<any>) => {
      state.type = "inviteToEvent";
      state.props = action.payload;
    },
    openEditEventModal: (
      state,
      action: PayloadAction<{
        eventId: string;
        initialData: {
          title: string;
          startDate: string;
          location: string;
          description: string;
        };
      }>
    ) => {
      state.type = "editEvent";
      state.props = action.payload;
    },
    closeModal: (state) => {
      state.type = null;
      state.props = {};
    },
  },
});

export const {
  openAddEventModal,
  openInviteToEventModal,
  openEditEventModal,
  closeModal,
} = modalSlice.actions;

export default modalSlice.reducer;
