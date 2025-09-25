import StatCard, { IStatCardProps } from "@components/StatCard/StatCard";
import styles from "./StatsList.module.scss";

interface IStatsListProps {
  events: IStatCardProps[];
  noEventsMessage?: string;
}

const StatsList = ({ events = [], noEventsMessage }: IStatsListProps) => {
  if (events.length === 0) {
    return <p>{noEventsMessage || "No events."}</p>;
  }

  return (
    <ul className={styles.EventsList}>
      {events.map((event, index) => (
        <StatCard key={`event-${index}`} {...event} />
      ))}
    </ul>
  );
};

export default StatsList;
