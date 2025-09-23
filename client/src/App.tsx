import { Routes, Route } from "react-router";
import Layout from "./components/Layout";
import HomePage from "@pages/Home";
import EventsPage from "@pages/Events";
import InvitesPage from "@pages/Invites";
import EventPage from "@pages/Event";
import StatsPage from "@pages/Stats";
import ProfilePage from "@pages/Profile";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<HomePage />} />
        <Route path="events" element={<EventsPage />} />
        <Route path="invites" element={<InvitesPage />} />
        <Route path="profile" element={<ProfilePage />} />
        <Route path="stats" element={<StatsPage />} />
        <Route path="profile" element={<ProfilePage />} />
        <Route path="events/:id" element={<EventPage />} />
      </Route>
    </Routes>
  );
}

export default App;
