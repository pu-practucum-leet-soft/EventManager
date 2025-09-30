import userQueries from "@queries/api/userQueries";
import styles from "./Sidebar.module.scss";
import SidebarElement from "./SidebarElement/SidebarElement";
import { useNavigate } from "react-router";

const Sidebar = () => {
  const navigate = useNavigate();

  const handleLogout = async () => {
    await userQueries.logout();

    navigate("/login");
  };

  return (
    <aside className={styles.Sidebar}>
      <SidebarElement to="/" icon="home" label="Home" />
      <SidebarElement to="/events" icon="calendar" label="Events" />
      <SidebarElement to="/invites" icon="user-plus" label="Invites" />
      <SidebarElement to="/stats" icon="chart" label="Stats" />
      <SidebarElement
        icon="right-from-bracket"
        label="Logout"
        onClick={handleLogout}
        className={styles.LogoutElement}
      />
    </aside>
  );
};

export default Sidebar;
