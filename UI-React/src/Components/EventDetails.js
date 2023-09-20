import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import useFetch from "../Hooks/useFetch";
import Button from "@mui/material/Button";
import {
  Typography,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Box,
  TextField,
  Alert,
  AlertTitle,
} from "@mui/material";

import { useState } from "react";
import RegistrationForm from "./Registrationform";
import eventService from "./EventService";
import InputFileUpload from "./UploadButton";
import { saveAs } from "file-saver";

function EventDetails() {
  const navigate = useNavigate();
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [nameError, setNameError] = useState("");
  const [emailError, setEmailError] = useState("");
  const eventId = useLocation().pathname.split("/")[2];

  const handleSubmit = (event) => {
    event.preventDefault();

    const isValid = validateForm();

    if (isValid) {
      console.log({ name, email });
      // TODO: Submit the form data to the server
    }
  };

  const validateForm = () => {
    let isValid = true;

    if (!name) {
      setNameError("Name is required");
      isValid = false;
    } else {
      setNameError("");
    }

    if (!email) {
      setEmailError("Email is required");
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(email)) {
      setEmailError("Invalid email address");
      isValid = false;
    } else {
      setEmailError("");
    }

    return isValid;
  };

  ///

  ////

  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleRegistration = async ({ name, email, eventId }) => {
    const subscriptionData = {
      eventId: eventId,
      email: email,
      name: name,
    };

    const response = await eventService.subscribe(subscriptionData);
    console.log(response);
    if (response) {
      alert("Registered successfully");
      navigate("/");
    }
  };

  const handleDownload = () => {
    const download = async () => {
      console.log(eventId);
      const response = await eventService.download(eventId);
      console.log(response.Headers);
      if (response) {
        const blob = new Blob([response.data], {
          type: "application/pdf",
        });
        saveAs(blob, "x.pdf");
        console.log("downloaded");
      }
    };
    download();
  };

  const id = eventId;
  const {
    data: event,
    isLoading,
    error,
  } = useFetch(`http://localhost:5160/Events/${id}`);

  if (error) {
    return <div className="error">{error}</div>;
  }

  if (isLoading) {
    return <div className="loading">Loading...</div>;
  }
  return (
    <div>
      <Paper sx={{ p: 2 }}>
        <Typography variant="h4">{event.name}</Typography>
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 500 }} aria-label="event details table">
            <TableBody>
              <TableRow>
                <TableCell>Id</TableCell>
                <TableCell>{event.id}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Date</TableCell>
                <TableCell>
                  {new Date(event.date).toLocaleDateString()}
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Mode</TableCell>
                <TableCell>{event.mode == 0 ? "Offline" : "Online"}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>
                  {event.mode == 0 ? "Venue" : " Joining Link"}
                </TableCell>
                <TableCell>{event.modelDetails}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Topic</TableCell>
                <TableCell>{event.topic}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Speakers</TableCell>
                <TableCell>{event.speakers}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Details</TableCell>
                <TableCell>{event.details}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Person of Contact</TableCell>
                <TableCell>{event.personOfContact}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Rules</TableCell>
                <TableCell>{event.rules}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Deadline</TableCell>
                <TableCell>
                  {new Date(event.deadline).toLocaleDateString()}
                </TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Community</TableCell>
                <TableCell>{event.community}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Capacity</TableCell>
                <TableCell>{event.capacity}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Type</TableCell>
                <TableCell>{event.type}</TableCell>
              </TableRow>
              <TableRow>
                <TableCell>Tags</TableCell>
                <TableCell>{event.tags}</TableCell>
              </TableRow>
            </TableBody>
          </Table>
        </TableContainer>
        <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
          {new Date(event.date) >= new Date() ? (
            <Button
              variant="solid"
              sx={{ backgroundColor: "#f1356d" }}
              onClick={handleOpen}
            >
              Register
            </Button>
          ) : (
            <>
              <InputFileUpload eventId={event.id} />
              <Button
                variant="solid"
                sx={{ backgroundColor: "#f1356d", ml: 3 }}
                onClick={handleDownload}
              >
                Download
              </Button>
            </>
          )}
          <RegistrationForm
            open={open}
            onClose={handleClose}
            eventId={eventId}
            onSubmit={handleRegistration}
          />
        </Box>
      </Paper>
    </div>
  );
}

export default EventDetails;
