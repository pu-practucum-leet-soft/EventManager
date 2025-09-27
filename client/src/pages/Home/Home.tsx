import { useQuery } from "@tanstack/react-query";
import homeQueries, { homeCacheTags } from "@queries/api/homeQueries";
import { eventParticipantViewModelsToCardProps } from "@utils/adapters/eventAdapter";

import EventsList from "@components/EventsList";
import InvitesList from "@components/InvitesList";
import Section from "@components/UI/Section";

import styles from "./Home.module.scss";

const HomePage = () => {
  const { data, isLoading, error } = useQuery({
    queryKey: [homeCacheTags.index],
    queryFn: async () => {
      const res = await homeQueries.getHome();

      return res.data;
    },
    retry: false,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Error loading data</div>;
  }

  return (
    <div className={styles.Home}>
      <h1 className={styles.Title}>Home Page</h1>
      <div className={styles.Content}>
        <div className={styles.Subsections}>
          {/* <section className={styles.UpcomingEvents}>Upcoming Events</section> */}
          <Section title="Upcoming Events" className={styles.UpcomingEvents}>
            <EventsList
              events={data.upcomingEvents}
              noEventsMessage="No upcoming events."
            />
          </Section>
          <Section title="Invites" className={styles.Invites}>
            <InvitesList
              invites={eventParticipantViewModelsToCardProps(data.invites)}
              noInvitesMessage="No invites available."
            />
          </Section>
        </div>
        <Section title="Recent Events" className={styles.RecentEvents}>
          <EventsList
            events={data.recentEvents}
            noEventsMessage="No recent events."
          />
        </Section>
      </div>
    </div>
  );
};

export default HomePage;
