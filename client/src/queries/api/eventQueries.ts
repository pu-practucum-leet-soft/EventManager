import axios from "@queries/axios";
import { EventViewModel } from "@queries/types/event.types";

const BASE_URL = `/events`;
const BASE_URL_INVITES = `/invites`;

type GetAllEventsResponse = {
  events: EventViewModel[];
};

type GetByIdResponse = {
  event: EventViewModel;
};

const eventQueries = {
  getAll: async () => {
    // TODO: adjust endpoint as needed
    return await axios.get<GetAllEventsResponse>(`${BASE_URL}/get-all`);
  },
  getById: async (id: string) => {
    return await axios.get<GetByIdResponse>(`${BASE_URL}/${id}`);
  },
  addEvent: async (eventData: {
    name: string;
    description: string;
    location: string;
    startDate: string;
  }) => {
    return await axios.post<EventViewModel>(`${BASE_URL}`, eventData);
  },
  editEvent: async (eventData: {
    eventId: string;
    title: string;
    description: string;
    location: string;
    startDate: string;
  }) => {
    return await axios.put<EventViewModel>(
      `${BASE_URL}/${eventData.eventId}`,
      eventData
    );
  },
  cancelEvent: async (eventId: string) => {
    return await axios.delete(`${BASE_URL}/${eventId}`);
  },
  inviteToEvent: (eventId: string, inviteeEmail: string) => {
    return axios.post(`${BASE_URL_INVITES}/${eventId}/add`, {
      inviteeEmail: inviteeEmail,
    });
  },
  unattendEvent: (eventId: string) => {
    return axios.delete(`${BASE_URL_INVITES}/${eventId}/unattend`);
  },
};

export const eventCacheTags = {
  index: "EVENTS",
};

export default eventQueries;
