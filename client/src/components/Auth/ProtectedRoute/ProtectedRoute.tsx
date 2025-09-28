import { ReactNode } from "react";
import { Navigate, useLocation } from "react-router";
import { useQuery } from "@tanstack/react-query";
import { setCredentials } from "@redux/slices/authSlice";
import userQueries from "@queries/api/userQueries";

import config from "@config";
const { routes } = config;

import { useDispatch, useSelector } from "react-redux";
import { jwtDecode } from "jwt-decode";

const ProtectedRoute = ({ children }: { children: ReactNode }) => {
  const dispatch = useDispatch();
  const location = useLocation();
  const token = useSelector((state: any) => state.auth.token);

  // Only run query if we're actually in a protected route context
  const shouldCheckAuth =
    location.pathname !== routes.login && location.pathname !== routes.register;

  const { data, isSuccess, isLoading, isError } = useQuery({
    queryKey: ["loggedUser"],
    queryFn: async () => {
      const response = await userQueries.refresh();

      const data = response.data;

      if (data.token) {
        const decodedToken: { sub: string; name: string } = jwtDecode(
          data.token
        );
        dispatch(
          setCredentials({
            accessToken: data.token,
            user: { userId: decodedToken.sub, username: decodedToken.name },
          })
        );
      }

      return response.data;
    },
    retry: false,
    refetchOnWindowFocus: false,
    enabled: shouldCheckAuth && !!!token, // Only check if we don't have a token yet
  });

  if (isError) {
    return <Navigate to={routes.login} state={{ from: location }} replace />;
  }

  if (isLoading) {
    return null;
  }

  if (!data || !isSuccess) {
    return <Navigate to={routes.login} state={{ from: location }} replace />;
  }

  return children;
};

export default ProtectedRoute;
