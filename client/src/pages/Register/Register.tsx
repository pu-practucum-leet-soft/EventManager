import Section from "@components/UI/Section";
import Button from "@components/UI/Button";

import { Link } from "react-router";
import styles from "./Register.module.scss";

const RegisterPage = () => {
  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle registration logic here
    alert("Registration functionality is not implemented yet.");
  };

  return (
    <div className={styles.Register}>
      <Section title="Register" className={styles.RegisterSection}>
        <form className={styles.Form} onSubmit={handleSubmit}>
          <div className={styles.FormGroup}>
            <label htmlFor="email">Email</label>
            <input type="text" id="email" name="email" />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="username">Username</label>
            <input type="text" id="username" name="username" />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="password">Password</label>
            <input type="password" id="password" name="password" />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="repassword">Repeat Password</label>
            <input type="password" id="repassword" name="repassword" />
          </div>
          <div className={styles.Actions}>
            <Link to="/login">
              <Button
                className={styles.LoginButton}
                variant="text"
                color="primary"
                border="rounded"
              >
                Login
              </Button>
            </Link>
            <Button
              variant="primary"
              color="primary"
              border="rounded"
              type="submit"
              className={styles.RegisterButton}
            >
              Register
            </Button>
          </div>
        </form>
      </Section>
    </div>
  );
};

export default RegisterPage;
