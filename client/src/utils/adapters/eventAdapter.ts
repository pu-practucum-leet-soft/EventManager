import {
  EventParticipantViewModel,
  EventViewModel,
} from "@queries/types/event.types";
import { IEventCardProps } from "@components/EventCard";
import { IInviteCardProps } from "@components/InviteCard";

export const eventViewModelToCardProps = (
  event: EventViewModel
): IEventCardProps => {
  return {
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
    inviterName: eventParticipant.inviter?.userName || "Unknown",
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
