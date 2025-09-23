import styles from "./Sidebar.module.scss";
import SidebarElement from "./SidebarElement/SidebarElement";

const Sidebar = () => {
  return (
    <aside className={styles.Sidebar}>
      <SidebarElement to="/" icon="home" label="Home" />
      <SidebarElement to="/events" icon="calendar" label="Events" />
      <SidebarElement to="/invites" icon="user-plus" label="Invites" />
    </aside>
  );
};

export default Sidebar;
