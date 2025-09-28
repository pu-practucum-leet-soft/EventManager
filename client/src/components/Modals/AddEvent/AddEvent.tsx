import Section from "@components/UI/Section";
import styles from "./AddEvent.module.scss";
import Button from "@components/UI/Button";
import { useState } from "react";

const AddEventModal = () => {
  const [eventData, setEventData] = useState({
    title: "",
    date: "",
    location: "",
    description: "",
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    console.log("Event Data:", eventData);
  };

  return (
    <div className={styles.AddEventModal}>
      <Section title="Add New Event">
        <form className={styles.AddEventForm} onSubmit={handleSubmit}>
          <input
            type="text"
            placeholder="Event Title"
            value={eventData.title}
            onChange={(e) =>
              setEventData({ ...eventData, title: e.target.value })
            }
          />
          <input
            type="date"
            placeholder="Event Date"
            value={eventData.date}
            onChange={(e) =>
              setEventData({ ...eventData, date: e.target.value })
            }
          />
          <input
            type="text"
            placeholder="Event Location"
            value={eventData.location}
            onChange={(e) =>
              setEventData({ ...eventData, location: e.target.value })
            }
          />
          <textarea
            placeholder="Event Description"
            value={eventData.description}
            onChange={(e) =>
              setEventData({ ...eventData, description: e.target.value })
            }
          ></textarea>
          <Button
            type="submit"
            variant="primary"
            color="primary"
            border="rounded"
          >
            Create Event
          </Button>
        </form>
      </Section>
    </div>
  );
};

export default AddEventModal;
