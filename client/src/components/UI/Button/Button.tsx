import { memo } from "react";
import classNames from "classnames";
import styles from "./Button.module.scss";

type ButtonVariant = "primary" | "secondary" | "text";
const variantMap: Record<ButtonVariant, string> = {
  primary: styles.PrimaryVariant,
  secondary: styles.SecondaryVariant,
  text: styles.TextVariant,
};

type ButtonColor = "primary" | "secondary" | "success" | "danger";
const colorMap: Record<ButtonColor, string> = {
  primary: styles.PrimaryColor,
  secondary: styles.SecondaryColor,
  success: styles.SuccessColor,
  danger: styles.DangerColor,
};

type ButtonBorder = "block" | "rounded" | "rounded-full";
const borderMap: Record<ButtonBorder, string> = {
  block: styles.BlockBorder,
  rounded: styles.RoundedBorder,
  "rounded-full": styles.RoundedFullBorder,
};

export interface IButtonProps {
  variant: ButtonVariant;
  color: ButtonColor;
  children: React.ReactNode;

  type?: "button" | "submit" | "reset";
  inline?: boolean;
  className?: string;
  border?: ButtonBorder;
  loader?: { isLoading: boolean; applyStyles?: boolean };
  onClick?: () => void;
}

const Button: React.FC<IButtonProps> = ({
  variant,
  color,
  children,
  onClick,
  className,
  border = "rounded",
  inline = false,
  type = "button",
  loader = { isLoading: false, applyStyles: false },
}) => {
  return (
    <button
      className={classNames(
        styles.BaseButton,
        className,
        variantMap[variant],
        colorMap[color],
        borderMap[border],
        {
          [styles.InlineButton]: inline,
          [styles.Loading]: loader.isLoading && loader.applyStyles,
        }
      )}
      onClick={onClick}
      type={type}
      disabled={loader.isLoading}
    >
      {children}
    </button>
  );
};

export default memo(Button);
