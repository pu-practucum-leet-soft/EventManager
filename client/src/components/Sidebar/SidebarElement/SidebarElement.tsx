import { Link } from "react-router";
import styles from "./SidebarElement.module.scss";
import iconMap from "../../UI/Icon/config/iconMap";
import Icon from "@components/UI/Icon";

interface ISidebarElementProps {
  to: string;
  icon: keyof typeof iconMap;
  label: string;
}

const SidebarElement: React.FC<ISidebarElementProps> = ({
  to,
  icon,
  label,
}) => {
  return (
    <Link className={styles.SidebarElement} to={to}>
      <span className={styles.Label}>{label}</span>
      <Icon name={icon} className={styles.Icon} />
    </Link>
  );
};

export default SidebarElement;
