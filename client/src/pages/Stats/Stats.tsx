import StatsList from "@components/StatsList";
import Section from "@components/UI/Section";

import styles from "./Stats.module.scss";
import { useQuery } from "@tanstack/react-query";
import eventQueries from "@queries/api/eventQueries";
import { statisticsToStatCardProps } from "@utils/adapters/eventAdapter";

// TODO: Maybe add participants list?
const StatsPage = () => {
  const {
    data: stats,
    isSuccess,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["statistics"],
    queryFn: async () => {
      const response = await eventQueries.getStatistics();

      console.log("Statistics response:", response.data);
      return response.data;
    },
    retry: false,
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isError || !isSuccess || !stats) {
    return <div>Error loading statistics</div>;
  }

  return (
    <div className={styles.Stats}>
      <h1>Statistics</h1>
      <Section title="Events" className={styles.StatsSection}>
        <div className={styles.StatsContent}>
          <p>Number of events: {stats.eventStatistics.length}</p>
          <StatsList
            events={statisticsToStatCardProps(stats.eventStatistics)}
          />
        </div>
      </Section>
    </div>
  );
};

export default StatsPage;
