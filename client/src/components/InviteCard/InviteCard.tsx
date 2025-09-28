import { useSelector } from "react-redux";
import styles from "./InviteCard.module.scss";
import invitesQueries, { invitesCacheTags } from "@queries/api/invitesQueries";
import { useQueryClient } from "@tanstack/react-query";
import { homeCacheTags } from "@queries/api/homeQueries";

export interface IInviteCardProps {
  eventId: string;
  eventTitle: string;
  inviterName: string;
  inviteeName: string;
  startDate: string;
  status?: "pending" | "accepted" | "declined";
}

const InviteCard: React.FC<IInviteCardProps> = ({
  eventId,
  eventTitle,
  inviterName,
  inviteeName,
  startDate,
  status,
}) => {
  const queryClient = useQueryClient();
  const currentUser = useSelector((state: any) => state.auth.user).username;
  const isInviter = inviterName === currentUser;

  const handleAccept = async () => {
    await invitesQueries.acceptInvite(eventId);
    queryClient.invalidateQueries({
      queryKey: [invitesCacheTags.index],
    });
    queryClient.invalidateQueries({
      queryKey: [homeCacheTags.index],
    });
  };

  const handleDecline = async () => {
    await invitesQueries.declineInvite(eventId);

    queryClient.invalidateQueries({
      queryKey: [invitesCacheTags.index],
    });
    queryClient.invalidateQueries({
      queryKey: [homeCacheTags.index],
    });
  };

  if (isInviter) {
    return (
      <div className={styles.InviteCard}>
        <h3 className={styles.EventTitle}>{eventTitle}</h3>
        <span className={styles.Inviter}>To: {inviteeName}</span>
        <span className={styles.Date}>
          {new Date(startDate).toLocaleDateString()}
        </span>
        <span>{status}</span>
      </div>
    );
  }

  return (
    <div className={styles.InviteCard}>
      <h3 className={styles.EventTitle}>{eventTitle}</h3>
      <span className={styles.Inviter}>From: {inviterName}</span>
      <span className={styles.Date}>
        {new Date(startDate).toLocaleDateString()}
      </span>
      <div className={styles.Actions}>
        <button className={styles.AcceptButton} onClick={handleAccept}>
          Accept
        </button>
        <button className={styles.DeclineButton} onClick={handleDecline}>
          Decline
        </button>
      </div>
    </div>
  );
};

export default InviteCard;
