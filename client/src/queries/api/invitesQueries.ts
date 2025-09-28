import axios from "@queries/axios";
import { EventParticipantViewModel } from "@queries/types/event.types";

// TODO: move to separate file when proper file structure is set up
type GetInvitesResponse = {
  incomingInvites: EventParticipantViewModel[];
  outcomingInvites: EventParticipantViewModel[];
};

const BASE_URL = `/invites`;

const invitesQueries = {
  getInvites: async () => {
    const response = await axios.get<GetInvitesResponse>(`${BASE_URL}`);
    return response;
  },
};

export const invitesCacheTags = {
  index: "INVITES",
};

export default invitesQueries;
