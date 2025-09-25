import InviteCard, { IInviteCardProps } from "@components/InviteCard";

import styles from "./InvitesList.module.scss";

interface IInvitesListProps {
  invites: IInviteCardProps[];
  noInvitesMessage?: string;
}

const InvitesList = ({ invites = [], noInvitesMessage }: IInvitesListProps) => {
  if (invites.length === 0) {
    return <p>{noInvitesMessage || "No invites."}</p>;
  }

  return (
    <ul className={styles.InvitesList}>
      {invites.map((invite, index) => (
        <InviteCard key={`invite-${index}`} {...invite} />
      ))}
    </ul>
  );
};

export default InvitesList;
