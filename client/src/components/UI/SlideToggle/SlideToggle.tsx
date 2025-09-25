import { useEffect, useRef, useState } from "react";

import styles from "./SlideToggle.module.scss";

interface ISlideToggleProps {
  isOpen: boolean;
  duration?: number;
  children: React.ReactNode;
}

const SlideToggle = ({
  isOpen,
  children,
  duration = 300,
}: ISlideToggleProps) => {
  const [shouldRender, setShouldRender] = useState(isOpen);
  const ref = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const el = ref.current;
    if (!el) return;

    if (isOpen) {
      setShouldRender(true);

      el.style.maxHeight = "0px";
      el.style.opacity = "0";

      setTimeout(() => {
        el.style.maxHeight = `500px`;
        el.style.opacity = "1";
      }, 10);
    } else {
      el.style.maxHeight = "0px";
      el.style.opacity = "0";

      const timeout = setTimeout(() => {
        setShouldRender(false);
      }, duration);

      return () => clearTimeout(timeout);
    }
  }, [isOpen, duration]);

  if (!shouldRender && !isOpen) return null;

  return (
    <div
      ref={ref}
      className={styles.Slide}
      style={{
        transition: `max-height ${duration}ms ease-in-out, opacity ${duration}ms ease`,
        overflow: "hidden",
      }}
    >
      {children}
    </div>
  );
};

export default SlideToggle;
