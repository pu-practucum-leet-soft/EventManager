import { useQuery } from "@tanstack/react-query";

import homeQueries, { homeCacheTags } from "@queries/api/homeQueries";

import styles from "./Home.module.scss";

const Home = () => {
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

  return <div className={styles.Home}>Home</div>;
};

export default Home;
