const config = {
  apiBaseUrl: import.meta.env.VITE_API_URL || "http://localhost:3000/api",
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
