import * as React from "react";
import { styled } from "@mui/material/styles";
import Button from "@mui/material/Button";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import eventService from "./EventService";
import { useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});

export default function InputFileUpload({ eventId }) {
  const navigate = useNavigate();

  const handleFileUpload = async (event) => {
    event.preventDefault();
    const file = event.target.files[0];
    const reader = new FileReader();
    reader.readAsArrayBuffer(file);
    reader.onload = async () => {
      const data = new Uint8Array(reader.result);
      const fileName = eventId + "|" + file.name;
      try {
        const formData = new FormData();
        formData.append("file", new Blob([data]), fileName);
        const response = await eventService.upload(formData);
        alert("File uploaded successfully");
        navigate("/");
      } catch (error) {
        alert("An error occurred while uploading the file. Please try again.");
      }
    };
  };

  return (
    <Button
      component="label"
      variant="contained"
      startIcon={<CloudUploadIcon />}
      sx={{ backgroundColor: "#f1356d" }}
    >
      Upload file
      <VisuallyHiddenInput type="file" onChange={handleFileUpload} />
    </Button>
  );
}
