import { Routes, Route } from "react-router";
import Layout from "./components/Layout";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Layout />}>
        <Route index element={<div>Home</div>} />
        <Route path="events" element={<div>Events</div>} />
        <Route path="invites" element={<div>Invites</div>} />
      </Route>
    </Routes>
  );
}

export default App;
