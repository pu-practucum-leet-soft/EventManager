import eventQueries, { eventCacheTags } from "./eventQueries";
import homeQueries, { homeCacheTags } from "./homeQueries";

const apiQueries = {
  homeQueries,
  eventQueries,
};

export const apiCacheTags = {
  homeCacheTags,
  eventCacheTags,
};

export default apiQueries;
