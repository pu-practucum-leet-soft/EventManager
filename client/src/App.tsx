import { Routes, Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./pages/Home/Home";
import Events from "./pages/Events/Events";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="events" element={<Events />} />
        <Route path="invites" element={<div>Invites</div>} />
      </Route>
    </Routes>
  );
}

export default App;
