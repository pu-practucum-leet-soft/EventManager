import { useSelector } from "react-redux";
import styles from "./InviteCard.module.scss";

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
  const currentUser = useSelector((state: any) => state.auth.user).username;
  const isInviter = inviterName === currentUser;

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
        <button className={styles.AcceptButton}>Accept</button>
        <button className={styles.DeclineButton}>Decline</button>
      </div>
    </div>
  );
};

export default InviteCard;
