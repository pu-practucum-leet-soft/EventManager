import Section from "@components/UI/Section";
import Button from "@components/UI/Button";

import { Link, useNavigate } from "react-router";
import styles from "./Register.module.scss";
import { useState } from "react";
import userQueries from "@queries/api/userQueries";
import config from "@config";
import { useQuery } from "@tanstack/react-query";

const RegisterPage = () => {
  const navigate = useNavigate();

  const { data, isSuccess, isLoading } = useQuery({
    queryKey: ["user"],
    queryFn: async () => {
      const response = await userQueries.refresh();

      return response.data;
    },
    retry: false,
    refetchOnWindowFocus: false,
  });

  if (isSuccess && data) {
    navigate(config.routes.home);
  }
  if (isLoading) {
    return null;
  }

  const [formData, setFormData] = useState({
    email: "",
    userName: "",
    password: "",
    repassword: "",
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    await userQueries.register(formData);

    navigate(config.routes.home);
  };

  return (
    <div className={styles.Register}>
      <Section title="Register" className={styles.RegisterSection}>
        <form className={styles.Form} onSubmit={handleSubmit}>
          <div className={styles.FormGroup}>
            <label htmlFor="email">Email</label>
            <input
              type="text"
              id="email"
              name="email"
              value={formData.email}
              onChange={(e) =>
                setFormData({ ...formData, email: e.target.value })
              }
            />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="username">Username</label>
            <input
              type="text"
              id="username"
              name="username"
              value={formData.userName}
              onChange={(e) =>
                setFormData({ ...formData, userName: e.target.value })
              }
            />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={(e) =>
                setFormData({ ...formData, password: e.target.value })
              }
            />
          </div>
          <div className={styles.FormGroup}>
            <label htmlFor="repassword">Repeat Password</label>
            <input
              type="password"
              id="repassword"
              name="repassword"
              value={formData.repassword}
              onChange={(e) =>
                setFormData({ ...formData, repassword: e.target.value })
              }
            />
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
