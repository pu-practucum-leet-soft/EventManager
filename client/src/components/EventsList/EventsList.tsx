import EventCard, { IEventCardProps } from "@components/EventCard";

import styles from "./EventsList.module.scss";

interface IEventListProps {
  events: IEventCardProps[];
  noEventsMessage?: string;
}

const EventsList = ({ events = [], noEventsMessage }: IEventListProps) => {
  if (events.length === 0) {
    return <p>{noEventsMessage || "No events."}</p>;
  }

  return (
    <ul className={styles.EventsList}>
      {events.map((event, index) => (
        <EventCard key={`event-${index}`} {...event} />
      ))}
    </ul>
  );
};

export default EventsList;
