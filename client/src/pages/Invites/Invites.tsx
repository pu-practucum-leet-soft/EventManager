import Section from "@components/UI/Section";
import styles from "./Invites.module.scss";
import InvitesList from "@components/InvitesList";
import { invites } from "@pages/dummyData";

const InvitesPage = () => {
  return (
    <div className={styles.Invites}>
      <h1>Invites</h1>
      <Section className={styles.InviteList}>
        <InvitesList invites={invites} />
      </Section>
    </div>
  );
};

export default InvitesPage;
