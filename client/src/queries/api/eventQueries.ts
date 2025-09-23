import axios from "@queries/axios";

const BASE_URL = `/events`;

const eventQueries = {
  getAll: async () => {
    // TODO: adjust endpoint as needed
    return await axios.get(`${BASE_URL}/get-all`);
  },
};

export const eventCacheTags = {
  index: "EVENTS",
};

export default eventQueries;
