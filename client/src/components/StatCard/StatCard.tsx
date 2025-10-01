import { useState } from "react";
import { Chart } from "react-google-charts";
import Icon from "@components/UI/Icon";

import styles from "./StatCard.module.scss";
import SlideToggle from "@components/UI/SlideToggle/SlideToggle";

export interface IStatCardProps {
  title: string;
  location: string;
  startDate: string;
  participantsCount: {
    accepted: number;
    declined: number;
    pending: number;
  };
}

const StatCard = ({
  title,
  location,
  startDate,
  participantsCount,
}: IStatCardProps) => {
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);

  const data = [
    ["Participants", "Status"],
    ["Pending", participantsCount.pending ?? 0],
    ["Accepted", participantsCount.accepted ?? 0],
    ["Declined", participantsCount.declined ?? 0],
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
        <span>{new Date(startDate).toLocaleDateString()}</span>
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
          <div className={styles.Summary}>
            <h3>Event Summary</h3>
            <p>Accepted: {participantsCount.accepted}</p>
            <p>Declined: {participantsCount.declined}</p>
            <p>Pending: {participantsCount.pending}</p>
          </div>
        </div>
      </SlideToggle>
    </div>
  );
};

export default StatCard;
