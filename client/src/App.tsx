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
import ProtectedRoute from "@components/Auth/ProtectedRoute";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={ProtectedRoute(<HomePage />)} />
        <Route path="events" element={ProtectedRoute(<EventsPage />)} />
        <Route path="invites" element={ProtectedRoute(<InvitesPage />)} />
        <Route path="profile" element={ProtectedRoute(<ProfilePage />)} />
        <Route path="stats" element={ProtectedRoute(<StatsPage />)} />
        <Route path="events/:id" element={ProtectedRoute(<EventPage />)} />
      </Route>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />
      <Route path="*" element={<h1>404 Not Found</h1>} />
    </Routes>
  );
}

export default App;
