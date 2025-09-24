import StatsList from "@components/StatsList";
import Section from "@components/UI/Section";
import { events } from "../dummyData";

import styles from "./Stats.module.scss";

// TODO: Maybe add participants list?
const StatsPage = () => {
  return (
    <div className={styles.Stats}>
      <h1>Statistics</h1>
      <Section title="Events" className={styles.StatsSection}>
        <div className={styles.StatsContent}>
          <p>Number of events: {events.length}</p>
          <StatsList events={events} />
        </div>
      </Section>
    </div>
  );
};

export default StatsPage;
