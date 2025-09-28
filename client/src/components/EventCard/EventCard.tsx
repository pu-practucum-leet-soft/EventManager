import { Link } from "react-router";
import styles from "./EventCard.module.scss";
import Button from "@components/UI/Button";

export interface IEventCardProps {
  id: string;
  title: string;
  location: string;
  startDate: string;
}

const EventCard = ({ id, title, location, startDate }: IEventCardProps) => {
  return (
    <div className={styles.EventCard}>
      <span className={styles.Title}>{title}</span>
      <span>{location}</span>
      <span>{new Date(startDate).toLocaleDateString()}</span>
      <Link to={`/events/${id}`}>
        <Button
          className={styles.ViewButton}
          inline
          variant="primary"
          color="primary"
          border="rounded"
        >
          View Details
        </Button>
      </Link>
    </div>
  );
};

export default EventCard;
