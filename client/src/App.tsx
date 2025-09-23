import { Routes, Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./pages/Home";
import Events from "./pages/Events";
import { getIsLoggedIn } from "@redux/slices/authSlice";
import { useSelector } from "react-redux";

function App() {
  const isLoggedIn = useSelector(getIsLoggedIn);

  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="events" element={<Events />} />
        <Route path="invites" element={<div>Invites</div>} />
        <Route
          path="profile"
          element={<div>{isLoggedIn ? "Logged In" : "Logged Out"}</div>}
        />
      </Route>
    </Routes>
  );
}

export default App;
