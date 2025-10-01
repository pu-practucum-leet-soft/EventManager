import Section from "@components/UI/Section";
import styles from "./InviteToEvent.module.scss";
import Button from "@components/UI/Button";
import eventQueries from "@queries/api/eventQueries";
import { useState } from "react";
import { useQueryClient, useMutation } from "@tanstack/react-query";
import { useAppDispatch } from "@redux/store";
import { closeModal } from "@redux/slices/modalSlice";

type InviteToEventModalProps = {
  eventId: string;
};

const InviteToEventModal: React.FC<InviteToEventModalProps> = ({ eventId }) => {
  const queryClient = useQueryClient();
  const dispatch = useAppDispatch();
  const [userEmail, setUserEmail] = useState("");

  const inviteMutate = useMutation({
    mutationFn: async (eventId: string) => {
      const response = await eventQueries.inviteToEvent(eventId, userEmail);

      return response.data;
    },
    onSuccess: () => {
      dispatch(closeModal());
      queryClient.invalidateQueries({ queryKey: ["event", eventId] });
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    inviteMutate.mutate(eventId);
  };
  return (
    <div className={styles.InviteToEventModal}>
      <Section title="Invite to Event">
        <form className={styles.InviteToEventForm} onSubmit={handleSubmit}>
          <p>
            Please enter the email address of the person you want to invite:
          </p>
          <input
            type="email"
            placeholder="Email address"
            value={userEmail}
            onChange={(e) => setUserEmail(e.target.value)}
          />
          <Button variant="primary" color="success" type="submit">
            Send
          </Button>
        </form>
      </Section>
    </div>
  );
};

export default InviteToEventModal;
