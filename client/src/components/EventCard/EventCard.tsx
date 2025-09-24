import { Link } from "react-router";
import styles from "./EventCard.module.scss";

export interface IEventCardProps {
  title: string;
  location: string;
  startDate: string;
  endDate: string;
}

const EventCard = ({
  title,
  location,
  startDate,
  endDate,
}: IEventCardProps) => {
  return (
    <div className={styles.EventCard}>
      <span className={styles.Title}>{title}</span>
      <span>{location}</span>
      <span>
        {startDate} - {endDate}
      </span>
      <Link to={`/events/${title}`}>View Details</Link>
    </div>
  );
};

export default EventCard;
