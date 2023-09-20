import React, { useState } from "react";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  Typography,
} from "@mui/material";
import * as yup from "yup";

const validationSchema = yup.object({
  name: yup.string().required("Name is required"),
  email: yup
    .string()
    .email("Invalid email address")
    .required("Email is required"),
});

function RegistrationForm({ open, onClose, onSubmit, eventId }) {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [Id, setId] = useState({ eventId });
  const [nameError, setNameError] = useState("");
  const [emailError, setEmailError] = useState("");

  const handleSubmit = (event) => {
    event.preventDefault();
    const isValid = validateForm();

    if (isValid) {
      onSubmit({ name, email, eventId });
      handleClose();
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

  const resetForm = () => {
    setName("");
    setEmail("");
    setId("");
    setNameError("");
    setEmailError("");
  };

  const handleClose = () => {
    resetForm();
    onClose();
  };

  return (
    <Dialog open={open} onClose={onClose}>
      <DialogTitle>Register for the event</DialogTitle>
      <DialogContent>
        <form onSubmit={handleSubmit}>
          {/* <TextField
            id="Id"
            name="Id"
            label="Id"
            variant="outlined"
            fullWidth
            margin="normal"
            value={eventId}
          /> */}
          <TextField
            id="name"
            name="name"
            label="Name"
            variant="outlined"
            fullWidth
            margin="normal"
            value={name}
            onChange={(event) => setName(event.target.value)}
            error={Boolean(nameError)}
            helperText={nameError}
          />
          <TextField
            id="email"
            name="email"
            label="Email"
            variant="outlined"
            fullWidth
            margin="normal"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            error={Boolean(emailError)}
            helperText={emailError}
          />
        </form>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} sx={{ color: "#f1356d" }}>
          Cancel
        </Button>
        <Button
          variant="solid"
          sx={{ backgroundColor: "#f1356d" }}
          type="submit"
          onClick={handleSubmit}
        >
          Sumit Request
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default RegistrationForm;
