import { useLocation } from "react-router";

const useCurrentRoute = (route: string) => {
  const location = useLocation();

  return location.pathname === route;
};

export default useCurrentRoute;
