import styles from "./InviteCard.module.scss";

export interface IInviteCardProps {
  eventId: string;
  eventTitle: string;
  inviterName: string;
  startDate: string;
}

const InviteCard: React.FC<IInviteCardProps> = ({
  eventId,
  eventTitle,
  inviterName,
  startDate,
}) => {
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
