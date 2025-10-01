const config = {
  apiBaseUrl: import.meta.env.VITE_API_URL || "https://localhost:7280/api",
  routes: {
    home: "/",
    events: "/events",
    invites: "/invites",
    profile: "/profile",
    login: "/login",
    register: "/register",
  },
};

export default config;
