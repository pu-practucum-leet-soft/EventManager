import EventsList from "@components/EventsList";
import InvitesList from "@components/InvitesList";
import Section from "@components/UI/Section";

import { events, invites } from "@pages/dummyData";

import styles from "./Home.module.scss";

const HomePage = () => {
  return (
    <div className={styles.Home}>
      <h1 className={styles.Title}>Home Page</h1>
      <div className={styles.Content}>
        <div className={styles.Subsections}>
          {/* <section className={styles.UpcomingEvents}>Upcoming Events</section> */}
          <Section title="Upcoming Events" className={styles.UpcomingEvents}>
            <EventsList events={events} noEventsMessage="No upcoming events." />
          </Section>
          <Section title="Invites" className={styles.Invites}>
            <InvitesList
              invites={invites}
              noInvitesMessage="No invites available."
            />
          </Section>
        </div>
        <Section title="Recent Events" className={styles.RecentEvents}>
          <EventsList events={events} noEventsMessage="No recent events." />
        </Section>
      </div>
    </div>
  );
};

export default HomePage;
