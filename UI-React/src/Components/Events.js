import EventList from "./EventList";
import useFetch from "../Hooks/useFetch";
import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { useParams } from "react-router-dom";

function Events() {
  // const { kind } = useParams();
  const [eventsState, setEventsState] = useState([]);
  const location = useLocation();
  const [kind, setKind] = useState("upcoming");
  console.log(kind);

  useEffect(() => {
    const { pathname } = location;
    const newKind = pathname.split("/")[2];
    setKind(newKind);
    console.log(newKind);
    if (newKind !== kind) {
      setEventsState([]);
    }
  }, [location]);

  const {
    data: events,
    isLoading,
    error,
  } = useFetch(`http://localhost:5160/Events?kind=${kind}`);

  useEffect(() => {
    if (events) {
      setEventsState(events);
    }
  }, [events]);

  return (
    <div className="home">
      {error && <div>{error}</div>}
      {isLoading && <p>Loading...</p>}
      {events && <EventList events={events} kind={kind} />}
    </div>
  );
}

export default Events;
