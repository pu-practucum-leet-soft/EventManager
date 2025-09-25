import { useParams } from "react-router";
import styles from "./Event.module.scss";
import Section from "@components/UI/Section/Section";
import Button from "@components/UI/Button";

export const EventPage = () => {
  const params = useParams();
  return (
    <div className={styles.Event}>
      <h1>{params.id}</h1>
      <div className={styles.EventContent}>
        <Section className={styles.Details}>
          <div className={styles.DetailContent}>
            <p>Details about event {params.id}...</p>
            <div className={styles.Actions}>
              <Button variant="primary" color="primary" border="rounded">
                Edit Event
              </Button>
              <Button variant="primary" color="danger" border="rounded">
                Delete Event
              </Button>
              <Button variant="primary" color="secondary" border="rounded">
                Share Event
              </Button>
            </div>
          </div>
        </Section>
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
