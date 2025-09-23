import { useParams } from "react-router";
import styles from "./Event.module.scss";

export const EventPage = () => {
  const params = useParams();
  return (
    <div className={styles.Event}>
      <h1>Event {params.id}</h1>
      <div className={styles.EventContent}>
        <section className={styles.Details}>
          <p>Details about event {params.id}...</p>
          <div className={styles.Actions}>
            <button>Edit Event</button>
            <button>Delete Event</button>
            <button>Share Event</button>
          </div>
        </section>
        <aside className={styles.Participants}>
          <h2>Participants</h2>
          <ul className={styles.ParticipantList}>
            <li>Participant 1</li>
            <li>Participant 2</li>
            <li>Participant 3</li>
          </ul>
        </aside>
      </div>
    </div>
  );
};

export default EventPage;
