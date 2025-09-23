import { ReactNode } from "react";
import { useSelector } from "react-redux";
import { getUser } from "@redux/slices/authSlice";

type OwnerOnlyProps = {
  ownerId: string;
  children: ReactNode;
};

const OwnerOnly: React.FC<OwnerOnlyProps> = ({ ownerId, children }) => {
  const isOwner = useSelector(getUser)?.userId === ownerId;
  if (!isOwner) return null;
  return <>{children}</>;
};

export default OwnerOnly;
