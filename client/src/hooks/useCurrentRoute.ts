import { useLocation } from "react-router";

const useCurrentRoute = (route: string) => {
  const location = useLocation();
  const firstSection = "/" + location.pathname.split("/")[1];

  return firstSection === route;
};

export default useCurrentRoute;
