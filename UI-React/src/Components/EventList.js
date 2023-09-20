import EventPreview from "./EventPreview";
import TextField from "@mui/material/TextField";
import { useState, useEffect, useLayoutEffect } from "react";

function EventList({ events, kind }) {
  const [inputText, setInputText] = useState("");
  let inputHandler = (e) => {
    var lowerCase = e.target.value.toLowerCase();
    setInputText(lowerCase);
  };
  useEffect(() => {
    setFilteredEvents([...events]);
  }, [events]);

  const [filteredEvents, setFilteredEvents] = useState([...events]);

  const filteredDataCall = useLayoutEffect(() => {
    setFilteredEvents((prevEvent) => {
      return events.filter((event) => {
        if (inputText === "") {
          return event;
        } else {
          return event.name.toLowerCase().includes(inputText);
        }
      });
    });
  }, [inputText]);

  return (
    <div className="blog-list">
      <h2
        style={{ paddingBottom: "16px", paddingLeft: "0.5px" }}
      >{`All ${kind} events`}</h2>
      <div className="search">
        <TextField
          id="outlined-basic"
          variant="outlined"
          onChange={inputHandler}
          fullWidth
          label="Search"
          sx={{ width: "105%" }}
        />
      </div>

      {filteredEvents.map((event) => (
        <EventPreview event={event} kind={kind} key={event.id} />
      ))}
    </div>
  );
}

export default EventList;
