import { Outlet } from "react-router";
import Sidebar from "../Sidebar";
import styles from "./Layout.module.scss";
import BaseModal from "@components/Modals";

const Layout = () => {
  return (
    <div className={styles.Layout}>
      <BaseModal />
      <Sidebar />
      <Outlet />
    </div>
  );
};

export default Layout;
