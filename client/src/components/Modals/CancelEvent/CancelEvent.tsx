import styles from "./CancelEvent.module.scss";
import Section from "@components/UI/Section";
import Button from "@components/UI/Button";
import { useAppDispatch } from "@redux/store";
import { closeModal } from "@redux/slices/modalSlice";
import { useMutation, useQueryClient } from "@tanstack/react-query";
import eventQueries from "@queries/api/eventQueries";

type CancelEventModalProps = {
  eventId: string;
};

const CancelEventModal: React.FC<CancelEventModalProps> = ({ eventId }) => {
  const dispatch = useAppDispatch();
  const queryClient = useQueryClient();

  const cancelMutate = useMutation({
    mutationFn: async () => {
      const response = await eventQueries.cancelEvent(eventId);

      return response.data;
    },
    onSuccess: () => {
      dispatch(closeModal());

      queryClient.invalidateQueries({ queryKey: ["event", eventId] });
    },
    onError: (error) => {
      console.error("Error cancelling event:", error);
    },
  });

  const handleCancel = () => {
    cancelMutate.mutate();
  };

  const handleBack = () => {
    dispatch(closeModal());
  };

  return (
    <div className={styles.CancelEventModal}>
      <Section title="Cancel Event">
        <p>Are you sure you want to cancel this event?</p>
        <div className={styles.ButtonGroup}>
          <Button variant="secondary" color="primary" onClick={handleBack}>
            No
          </Button>
          <Button variant="primary" color="danger" onClick={handleCancel}>
            Yes
          </Button>
        </div>
      </Section>
    </div>
  );
};

export default CancelEventModal;
