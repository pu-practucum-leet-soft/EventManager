import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import styles from "./Events.module.scss";
import { useQuery } from "@tanstack/react-query";
import classNames from "classnames";

const EventsPage = () => {
  const { data, isSuccess, isLoading } = useQuery({
    queryKey: [eventCacheTags.index],
    queryFn: async () => {
      console.log("Fetching event data...");
      const res = await eventQueries.getAll();
      return res.data;
    },
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isSuccess) {
    console.log(data);
  }

  // TODO: Example rendering of events, adjust as needed when actual data structure is known
  return (
    <div className={styles.Events}>
      <h1>Events</h1>
      <div className={styles.EventsContainer}>
        <div className={styles.FilterBar}>
          <input
            className={classNames(styles.FilterField, styles.SearchField)}
            type="text"
            placeholder="Search events..."
          />
          <select className={styles.FilterField} name="location" id="">
            <option value="">Select Location</option>
            <option value="location1">Location 1</option>
            <option value="location2">Location 2</option>
          </select>
          <input
            className={styles.FilterField}
            type="date"
            placeholder="Select Date"
          />
          <button className={styles.FilterButton}>Filter</button>
        </div>
        {isSuccess && data ? (
          <ul className={styles.EventList}>
            {data.events.map((event, index) => (
              <li key={`event-${index}`}>{event.name}</li>
            ))}
          </ul>
        ) : (
          <p>No events available.</p>
        )}
        <div className={styles.Actions}>
          <div className={styles.Pagination}>
            <button className={styles.PageButton}>Previous</button>
            <span className={styles.PageInfo}>Page 1 of 10</span>
            <button className={styles.PageButton}>Next</button>
          </div>
          <div className={styles.AddEvent}>
            <button className={styles.AddEventButton}>Add New Event</button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EventsPage;
