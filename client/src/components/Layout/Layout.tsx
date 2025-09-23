import { Outlet } from "react-router";
import Sidebar from "../Sidebar";
import styles from "./Layout.module.scss";

const Layout = () => {
  return (
    <div className={styles.Layout}>
      <Sidebar />
      <Outlet />
    </div>
  );
};

export default Layout;
