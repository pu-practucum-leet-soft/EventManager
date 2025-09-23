import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import styles from "./Events.module.scss";
import { useQuery } from "@tanstack/react-query";

const Events = () => {
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
      {data.events.map((event: any, index: number) => (
        <div key={`event-${index}`} className={styles.EventCard}>
          <h3>{event.title}</h3>
          <p>{event.description}</p>
          <p>Date: {new Date(event.date).toLocaleDateString()}</p>
          <p>Location: {event.location}</p>
        </div>
      ))}
    </div>
  );
};

export default Events;
