import axios from "@queries/axios";
import {
  EventParticipantViewModel,
  EventViewModel,
} from "@queries/types/event.types";

// TODO: move to separate file when proper file structure is set up
type GetHomeResponse = {
  upcomingEvents: EventViewModel[];
  recentEvents: EventViewModel[];
  invites: EventParticipantViewModel[];
};

const BASE_URL = `/home`;

const homeQueries = {
  getHome: async () => {
    const response = await axios.get<GetHomeResponse>(`${BASE_URL}`);
    return response;
  },
};

export const homeCacheTags = {
  index: "HOME",
};

export default homeQueries;
