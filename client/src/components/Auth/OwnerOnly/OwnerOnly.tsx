import { ReactNode } from "react";
import { useSelector } from "react-redux";
import { getUser } from "@redux/slices/authSlice";

type OwnerOnlyProps = {
  userId: string;
  children: ReactNode;
};

const OwnerOnly: React.FC<OwnerOnlyProps> = ({ userId, children }) => {
  const currentUser = useSelector(getUser);

  if (currentUser?.userId !== userId) return null;

  return <>{children}</>;
};

export default OwnerOnly;
