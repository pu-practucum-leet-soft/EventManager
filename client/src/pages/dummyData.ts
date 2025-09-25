import { IEventCardProps } from "@components/EventCard";
import { IInviteCardProps } from "@components/InviteCard";

export const events: IEventCardProps[] = [
  {
    title: "PU Praktika Project Defense",
    location: "Plovdiv, Bulgaria",
    startDate: "2025-09-28",
    endDate: "2025-09-28",
  },
  {
    title: "PU Izbiraema Disciplina 1",
    location: "Plovdiv, Bulgaria",
    startDate: "2025-09-27",
    endDate: "2025-09-28",
  },
  {
    title: "PU Izbiraema Disciplina 2",
    location: "Plovdiv, Bulgaria",
    startDate: "2025-09-29",
    endDate: "2025-09-30",
  },
];

export const invites: IInviteCardProps[] = [
  {
    eventId: "1",
    eventTitle: "Birthday Party",
    inviterName: "Alice",
    date: "2025-10-05",
  },
  {
    eventId: "2",
    eventTitle: "Conference",
    inviterName: "Bob",
    date: "2025-11-15",
  },
];
