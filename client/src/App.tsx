import { Routes, Route } from "react-router";
import Layout from "./components/Layout";
import HomePage from "@pages/Home";
import EventsPage from "@pages/Events";
import InvitesPage from "@pages/Invites";
import EventPage from "@pages/Event";
import StatsPage from "@pages/Stats";
import ProfilePage from "@pages/Profile";
import LoginPage from "@pages/Login/Login";
import RegisterPage from "@pages/Register/Register";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<HomePage />} />
        <Route path="events" element={<EventsPage />} />
        <Route path="invites" element={<InvitesPage />} />
        <Route path="profile" element={<ProfilePage />} />
        <Route path="stats" element={<StatsPage />} />
        <Route path="events/:id" element={<EventPage />} />
      </Route>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="*" element={<h1>404 Not Found</h1>} />
    </Routes>
  );
}

export default App;
