import Section from "@components/UI/Section";

import styles from "./Login.module.scss";
import { Link } from "react-router";
import Button from "@components/UI/Button/Button";

const LoginPage = () => {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle login logic here
    alert("Login functionality is not implemented yet.");
  };

  return (
    <div className={styles.Login}>
      <Section title="Login" className={styles.LoginSection}>
        <form className={styles.Form} onSubmit={handleSubmit}>
          <div className={styles.FormGroup}>
            <label htmlFor="username">Username</label>
            <input type="text" id="username" name="username" />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="password">Password</label>
            <input type="password" id="password" name="password" />
          </div>
          <div className={styles.Actions}>
            <Link to="/register">
              <Button
                className={styles.RegisterButton}
                variant="text"
                color="primary"
                border="rounded"
              >
                Register
              </Button>
            </Link>
            <Button
              variant="primary"
              color="primary"
              border="rounded"
              type="submit"
              className={styles.LoginButton}
            >
              Login
            </Button>
          </div>
        </form>
      </Section>
    </div>
  );
};

export default LoginPage;
