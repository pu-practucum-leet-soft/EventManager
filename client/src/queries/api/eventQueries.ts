import axios from "@queries/axios";
import { EventViewModel } from "@queries/types/event.types";

const BASE_URL = `/events`;

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
    console.log(`${BASE_URL}/${eventData.eventId}`);
    console.log(eventData);
    return await axios.put<EventViewModel>(
      `${BASE_URL}/${eventData.eventId}`,
      eventData
    );
  },
};

export const eventCacheTags = {
  index: "EVENTS",
};

export default eventQueries;
