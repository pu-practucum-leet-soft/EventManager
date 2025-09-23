import styles from "./Invites.module.scss";

const InvitesPage = () => {
  return (
    <div className={styles.Invites}>
      <h1>Invites</h1>
      <ul className={styles.InviteList}>
        <li>Invite 1</li>
        <li>Invite 2</li>
        <li>Invite 3</li>
      </ul>
    </div>
  );
};

export default InvitesPage;
