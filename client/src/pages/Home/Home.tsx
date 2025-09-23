import { useQuery } from "@tanstack/react-query";

import homeQueries, { homeCacheTags } from "@queries/api/homeQueries";

import styles from "./Home.module.scss";

const HomePage = () => {
  const { data, isSuccess, isLoading } = useQuery({
    queryKey: [homeCacheTags.index],
    queryFn: async () => {
      console.log("Fetching home data...");
      const res = await homeQueries.getHome();
      return res.data;
    },
  });

  if (isLoading) {
    return <div>Loading...</div>;
  }

  if (isSuccess) {
    console.log(data);
  }

  return (
    <div className={styles.Home}>
      <h1>Home Page</h1>
      <div className={styles.Content}>
        <div className={styles.Subsections}>
          <section className={styles.UpcomingEvents}>Upcoming Events</section>
          <section className={styles.Invites}>Invites</section>
        </div>
        <section className={styles.RecentEvents}>Recent Events</section>
      </div>
    </div>
  );
};

export default HomePage;
