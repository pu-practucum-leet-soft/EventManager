import axios from "@queries/axios";

const BASE_URL = `/home`;

const homeQueries = {
  getHome: async () => {
    const response = await axios.get(`${BASE_URL}`);
    return response;
  },
};

export const homeCacheTags = {
  index: "HOME",
};

export default homeQueries;
