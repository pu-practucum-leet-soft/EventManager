import Section from "@components/UI/Section";
import styles from "./EditEvent.module.scss";
import Button from "@components/UI/Button";
import { useState } from "react";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import eventQueries, { eventCacheTags } from "@queries/api/eventQueries";
import { useAppDispatch } from "@redux/store";
import { closeModal } from "@redux/slices/modalSlice";

type EditEventModalProps = {
  eventId: string;
  initialData: {
    title: string;
    startDate: string;
    location: string;
    description: string;
  };
};

const EditEventModal = ({ eventId, initialData }: EditEventModalProps) => {
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  console.log(initialData);
  const [eventData, setEventData] = useState({
    title: initialData.title,
    startDate: initialData.startDate,
    location: initialData.location,
    description: initialData.description,
  });

  const editMutate = useMutation({
    mutationKey: ["editEvent"],
    mutationFn: async (updatedEvent: {
      eventId: string;
      name: string;
      description: string;
      location: string;
      startDate: string;
    }) => {
      return await eventQueries.editEvent(updatedEvent);
    },
    onSuccess: (data) => {
      console.log(data);
      queryClient.invalidateQueries({ queryKey: [eventCacheTags.index] });
      dispatch(closeModal());
    },
    onError: (error) => {
      // Handle error (e.g., show error message)
      console.error("Error editing event:", error);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    // console.log("Event Data:", eventData);
    editMutate.mutate({
      eventId: eventId,
      name: eventData.title,
      description: eventData.description,
      location: eventData.location,
      startDate: eventData.startDate,
    });
  };
  const testDate = new Date(eventData.startDate).toISOString().split("T")[0];
  return (
    <div className={styles.EditEventModal}>
      <Section title="Edit Event">
        <form className={styles.EditEventForm} onSubmit={handleSubmit}>
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
            placeholder="Event Start Date"
            value={testDate}
            onChange={(e) =>
              setEventData({ ...eventData, startDate: e.target.value })
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
            Edit Event
          </Button>
        </form>
      </Section>
    </div>
  );
};

export default EditEventModal;
