import { useState } from "react";
import { Chart } from "react-google-charts";
import Icon from "@components/UI/Icon";

import styles from "./StatCard.module.scss";
import SlideToggle from "@components/UI/SlideToggle/SlideToggle";

export interface IStatCardProps {
  title: string;
  location: string;
  startDate: string;
  endDate: string;
}

const StatCard = ({ title, location, startDate, endDate }: IStatCardProps) => {
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);

  const data = [
    ["Participants", "Status"],
    ["Pending", 11],
    ["Accepted", 13],
    ["Declined", 7],
  ];

  const options = {
    title: "Participant Status",
    pieHole: 0.4,
    is3D: false,
    backgroundColor: "transparent",
    colors: ["#ffab00", "#36b37e", "#ff5630"],
  };

  const handleClick = () => {
    setIsDetailsOpen(!isDetailsOpen);
  };

  return (
    <div className={styles.StatCard}>
      <button className={styles.Header} onClick={handleClick}>
        <span className={styles.Title}>{title}</span>
        <span>{location}</span>
        <span>
          {startDate} - {endDate}
        </span>
        <Icon name="arrow-down" className={styles.Icon} />
      </button>
      <SlideToggle isOpen={isDetailsOpen} duration={300}>
        <div className={styles.Details}>
          <Chart
            chartType="PieChart"
            data={data}
            options={options}
            width={"600px"}
            height={"500px"}
          />
        </div>
      </SlideToggle>
    </div>
  );
};

export default StatCard;
