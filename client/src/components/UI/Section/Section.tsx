import classNames from "classnames";

import styles from "./Section.module.scss";

interface ISectionProps {
  children: React.ReactNode;
  title?: string;
  className?: string;
}
const Section = ({ title, children, className }: ISectionProps) => {
  return (
    <section className={classNames(styles.Section, className)}>
      {title && <h2 className={styles.Title}>{title}</h2>}
      <div className={styles.Content}>{children}</div>
    </section>
  );
};

export default Section;
