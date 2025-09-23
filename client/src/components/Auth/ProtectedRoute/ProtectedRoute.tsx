import { ReactNode } from "react";
import { Navigate, useLocation } from "react-router";
import { useQuery } from "@tanstack/react-query";
import { setUser } from "@redux/slices/authSlice";
import apiQueries from "@queries/api";
import config from "@config";
const { routes } = config;

import { useDispatch } from "react-redux";

const ProtectedRoute = (children: ReactNode) => {
  const dispatch = useDispatch();
  const location = useLocation();

  //? Add back later when Auth is fully functional
  // Only run query if we're actually in a protected route context
  const shouldCheckAuth =
    location.pathname !== routes.login && location.pathname !== routes.register;

  // const { data, isSuccess, isLoading } = useQuery({
  //   queryKey: ['loggedUser'],
  //   queryFn: async () => {
  //     const response = await apiQueries.authQueries.getUser();
  //     return response.data;
  //   },
  //   retry: false,
  //   refetchOnWindowFocus: false,
  //   enabled: shouldCheckAuth,
  // });

  // if (isLoading) {
  //   return <div>Loading in...</div>;
  // }

  // if (!data || !isSuccess) {
  //   return <Navigate to={routes.LOGIN} state={{ from: location }} replace />;
  // }

  // dispatch(setUser(data));

  return children;
};

export default ProtectedRoute;
