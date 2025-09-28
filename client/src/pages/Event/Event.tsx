import { useParams } from "react-router";
import styles from "./Event.module.scss";
import Section from "@components/UI/Section/Section";
import Button from "@components/UI/Button";
import { useQuery } from "@tanstack/react-query";
import eventQueries from "@queries/api/eventQueries";
import OwnerOnly from "@components/Auth/OwnerOnly";
import { useAppDispatch } from "@redux/store";
import { openEditEventModal } from "@redux/slices/modalSlice";

export const EventPage = () => {
  const params = useParams();
  const dispatch = useAppDispatch();

  const {
    data: event,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["event", params.id],
    queryFn: async () => {
      const response = await eventQueries.getById(params.id!);

      return response.data.event;
    },
    retry: false,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error loading data</div>;
  }

  if (!event) {
    return <div>No event found</div>;
  }
  console.log(event);

  const handleEdit = () => {
    dispatch(
      openEditEventModal({
        eventId: event.id,
        initialData: {
          title: event.title,
          startDate: event.startDate,
          location: event.location,
          description: event.description,
        },
      })
    );
  };

  return (
    <div className={styles.Event}>
      <h1>{event.title}</h1>
      <div className={styles.EventContent}>
        <Section className={styles.Details}>
          <div className={styles.DetailContent}>
            <p>{event.description}</p>
            <p>Location: {event.location}</p>
            <p>
              Start Date:{" "}
              {new Date(event.startDate).toLocaleDateString("en-US", {
                year: "numeric",
                month: "long",
                day: "numeric",
                hour: "2-digit",
                minute: "2-digit",
              })}
            </p>
            <div className={styles.Actions}>
              <OwnerOnly userId={event.ownerUserId}>
                <Button
                  variant="primary"
                  color="primary"
                  border="rounded"
                  onClick={handleEdit}
                >
                  Edit Event
                </Button>
                <Button variant="primary" color="danger" border="rounded">
                  Delete Event
                </Button>
              </OwnerOnly>
              <Button variant="primary" color="secondary" border="rounded">
                Share Event
              </Button>
            </div>
          </div>
        </Section>
        <aside className={styles.Participants}>
          <h2>Participants</h2>
          <ul className={styles.ParticipantList}>
            {event.participants && event.participants.length > 0 ? (
              event.participants.map((participant, index) => (
                <li key={index} className={styles.ParticipantItem}>
                  <span className={styles.ParticipantName}>
                    {participant.invitee?.userName || "Unknown User"}
                  </span>
                </li>
              ))
            ) : (
              <li>No participants found.</li>
            )}
          </ul>
        </aside>
      </div>
    </div>
  );
};

export default EventPage;
