import Section from "@components/UI/Section";
import styles from "./AddEvent.module.scss";
import Button from "@components/UI/Button";
import { useState } from "react";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import { useAppDispatch } from "@redux/store";
import { closeModal } from "@redux/slices/modalSlice";

const AddEventModal = () => {
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  const [eventData, setEventData] = useState({
    title: "",
    date: "",
    location: "",
    description: "",
  });

  const addMutate = useMutation({
    mutationKey: ["addEvent"],
    mutationFn: async (newEvent: {
      name: string;
      description: string;
      location: string;
      startDate: string;
    }) => {
      // Call the API to add the event
      return await eventQueries.addEvent(newEvent);
    },
    onSuccess: (data) => {
      console.log(data);
      // Handle success (e.g., close modal, show success message, refresh event list)
      queryClient.invalidateQueries({ queryKey: [eventCacheTags.index] });
      dispatch(closeModal());
    },
    onError: (error) => {
      // Handle error (e.g., show error message)
      console.error("Error adding event:", error);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // console.log("Event Data:", eventData);
    addMutate.mutate({
      name: eventData.title,
      description: eventData.description,
      location: eventData.location,
      startDate: eventData.date,
    });
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
