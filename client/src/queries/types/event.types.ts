import { UserViewModel } from "./user.types";

export type EventViewModel = {
  id: string;
  title: string;
  description: string;
  startDate: string; // ISO string
  location: string;
  ownerUserId: string;
  status: number;
  participants?: EventParticipantViewModel[];
};

export type EventParticipantViewModel = {
  id: string;
  event: EventViewModel;
  invitee?: UserViewModel;
  inviter?: UserViewModel;
  status: "Invited" | "Accepted" | "Declined";
};
