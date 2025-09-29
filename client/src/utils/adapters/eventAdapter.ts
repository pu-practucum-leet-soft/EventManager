import {
  EventParticipantViewModel,
  EventViewModel,
} from "@queries/types/event.types";
import { IEventCardProps } from "@components/EventCard";
import { IInviteCardProps } from "@components/InviteCard";

const statusMap: { [key: string]: "pending" | "accepted" | "declined" } = {
  0: "pending",
  1: "accepted",
  2: "declined",
};

export const eventViewModelToCardProps = (
  event: EventViewModel
): IEventCardProps => {
  return {
    id: event.id,
    title: event.title,
    location: event.location,
    startDate: event.startDate,
  };
};

export const eventParticipantViewModelToCardProps = (
  eventParticipant: EventParticipantViewModel
): IInviteCardProps => {
  return {
    eventId: eventParticipant.event.id,
    eventTitle: eventParticipant.event.title,
    startDate: eventParticipant.event.startDate,
    inviteeName: eventParticipant.invitee?.userName || "Unknown",
    inviterName: eventParticipant.inviter?.userName || "Unknown",
    status: statusMap[eventParticipant.status] || "unknown",
  };
};

export const eventParticipantViewModelsToCardProps = (
  eventParticipants: EventParticipantViewModel[]
): IInviteCardProps[] => {
  return eventParticipants.map(eventParticipantViewModelToCardProps);
};

export const eventViewModelsToCardProps = (
  events: EventViewModel[]
): IEventCardProps[] => {
  return events.map(eventViewModelToCardProps);
};

export type EventStatus = "active" | "canceled" | "archived";
export const eventStatusMap: { [key: number]: EventStatus } = {
  0: "active",
  1: "canceled",
  2: "archived",
};
