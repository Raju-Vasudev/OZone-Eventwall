import Navbar from "./Components/Navbar";
import Events from "./Components/Events";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import EventDetails from "./Components/EventDetails";
import { Navigate } from "react-router-dom";
import CreateEvent from "./Components/CreateEvent";
import EventSuggestion from "./Components/EventSuggestion";

function App() {
  return (
    <Router>
      <div className="App">
        <Navbar />
        <div className="content">
          <Routes>
            <Route
              exact
              path="/"
              element={<Navigate to="/events/upcoming" />}
            ></Route>
            <Route path="/events/upcoming" element={<Events />}></Route>
            <Route path="/events/past" element={<Events kind="past" />}></Route>
            <Route path="/events/suggest" element={<EventSuggestion/>}></Route>
            <Route path="/events/create" element={<CreateEvent />}></Route>
            <Route path="/event/:Id" element={<EventDetails />}></Route>
            {/* <Route path="*" element={<Navigate to="/" />} /> */}
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
