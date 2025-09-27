import { ReactNode } from "react";
import { Navigate, useLocation } from "react-router";
import { useQuery } from "@tanstack/react-query";
import { setCredentials } from "@redux/slices/authSlice";
import userQueries from "@queries/api/userQueries";

import config from "@config";
const { routes } = config;

import { useDispatch } from "react-redux";

const ProtectedRoute = (children: ReactNode) => {
  const dispatch = useDispatch();
  const location = useLocation();

  // Only run query if we're actually in a protected route context
  const shouldCheckAuth =
    location.pathname !== routes.login && location.pathname !== routes.register;

  const { data, isSuccess, isLoading, isError } = useQuery({
    queryKey: ["loggedUser"],
    queryFn: async () => {
      const response = await userQueries.refresh();

      return response.data;
    },
    retry: false,
    refetchOnWindowFocus: false,
    enabled: shouldCheckAuth,
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

  dispatch(setCredentials({ accessToken: data.token }));

  return children;
};

export default ProtectedRoute;
