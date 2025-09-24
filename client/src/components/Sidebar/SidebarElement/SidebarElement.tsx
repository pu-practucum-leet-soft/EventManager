import { Link } from "react-router";
import styles from "./SidebarElement.module.scss";
import iconMap from "../../UI/Icon/config/iconMap";
import Icon from "@components/UI/Icon";
import useCurrentRoute from "@hooks/useCurrentRoute";
import classNames from "classnames";

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
  const isCurrentRoute = useCurrentRoute(to);

  return (
    <Link
      className={classNames(styles.SidebarElement, {
        [styles.Active]: isCurrentRoute,
      })}
      to={to}
    >
      <span className={styles.Label}>{label}</span>
      <Icon name={icon} className={styles.Icon} />
    </Link>
  );
};

export default SidebarElement;
