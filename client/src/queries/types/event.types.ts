import { UserViewModel } from "./user.types";

export type EventViewModel = {
  id: string;
  title: string;
  description: string;
  startDate: string; // ISO string
  location: string;
  createdBy: {
    id: string;
    name: string;
    email: string;
  };
};

export type EventParticipantViewModel = {
  id: string;
  event: EventViewModel;
  invitee?: UserViewModel;
  inviter?: UserViewModel;
  status: "Invited" | "Accepted" | "Declined";
};
