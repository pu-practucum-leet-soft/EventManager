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
};

export const eventCacheTags = {
  index: "EVENTS",
};

export default eventQueries;
