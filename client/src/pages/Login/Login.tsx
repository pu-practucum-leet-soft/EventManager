import { useState } from "react";
import { Link, useNavigate } from "react-router";

import config from "@config";
import userQueries from "@queries/api/userQueries";

import Section from "@components/UI/Section";
import Button from "@components/UI/Button/Button";
import styles from "./Login.module.scss";

const LoginPage = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Handle login logic here
    const res = await userQueries.login({ email, password });

    console.log(res.data);

    navigate(config.routes.home);
  };

  return (
    <div className={styles.Login}>
      <Section title="Login" className={styles.LoginSection}>
        <form className={styles.Form} onSubmit={handleSubmit}>
          <div className={styles.FormGroup}>
            <label htmlFor="username">Username</label>
            <input
              type="text"
              id="username"
              name="username"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
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
