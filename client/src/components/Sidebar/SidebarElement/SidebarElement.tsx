import { Link } from "react-router";
import styles from "./SidebarElement.module.scss";
import iconMap from "../../UI/Icon/config/iconMap";
import Icon from "@components/UI/Icon";
import useCurrentRoute from "@hooks/useCurrentRoute";
import classNames from "classnames";

interface ISidebarElementProps {
  icon: keyof typeof iconMap;
  label: string;
  to?: string;
  onClick?: () => void;
  className?: string;
}

const SidebarElement: React.FC<ISidebarElementProps> = ({
  to,
  icon,
  label,
  onClick,
  className,
}) => {
  const isCurrentRoute = useCurrentRoute(to ?? "");

  return to ? (
    <Link
      className={classNames(styles.SidebarElement, className, {
        [styles.Active]: isCurrentRoute,
      })}
      to={to}
    >
      <span className={styles.Label}>{label}</span>
      <Icon name={icon} className={styles.Icon} />
    </Link>
  ) : (
    <button
      className={classNames(styles.SidebarElement, className)}
      onClick={onClick}
    >
      <span className={styles.Label}>{label}</span>
      <Icon name={icon} className={styles.Icon} />
    </button>
  );
};

export default SidebarElement;
