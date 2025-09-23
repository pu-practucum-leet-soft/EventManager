import axios from "@queries/axios";

const BASE_URL = `/events`;

const eventQueries = {
  getAll: async () => {
    return await axios.get(`${BASE_URL}`);
  },
};

export const eventCacheTags = {
  index: "EVENTS",
};

export default eventQueries;
