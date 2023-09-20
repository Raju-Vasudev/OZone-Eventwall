import { useState } from "react";
import EventPreview from "./EventPreview";
import eventService from "./EventService";
import { Button } from "@mui/material";
import "./EventSuggestion.css"
import {TextField} from "@mui/material";

function EventSuggestion() {
    const [events, setEvents] = useState(null);
    const [error, setError] = useState(null);
    const [community, setCommunity] = useState("");
    const [email, setEmail] = useState("");
    const [communityError, setCommunityError] = useState("");
    const [emailError, setEmailError] = useState("");

    const handleSuggestEvent = async (event) => {
        event.preventDefault();

        const isValid = validateForm();

        if (!isValid) {
            return;
        }

        const suggestEventPayload = {
            community: community,
            email: email
        };

        try {
            const response = await eventService.suggestEvent(suggestEventPayload);
            console.log("response from suggest events api: ");
            console.log(response);
            setEvents(response);
            setError(null);
        } catch (e) {
            console.log("create event request failed", e);
            setError("Some problem while getting suggested topics")
        }
    }

    const validateForm = () => {
        let isValid = true;

        if (!community) {
            setCommunityError("Community is required");
            isValid = false;
        } else {
            setCommunityError("");
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

    return (
        <div className="home">
            {/* {isLoading && <p>Loading...</p>} */}
                <TextField
                    id="community"
                    name="community"
                    label="Communit / Area of interest"
                    variant="outlined"
                    style={{width: "50%"}}
                    margin="normal"
                    value={community}
                    onChange={(event) => setCommunity(event.target.value)}
                    error={Boolean(communityError)}
                    helperText={communityError}
                />
                <br/>
                <TextField
                    id="email"
                    name="email"
                    label="Email"
                    variant="outlined"
                    style={{width: "50%"}}
                    margin="normal"
                    value={email}
                    onChange={(event) => setEmail(event.target.value)}
                    error={Boolean(emailError)}
                    helperText={emailError}
                />
            <div>
                <Button
                    variant="solid"
                    sx={{ backgroundColor: "#f1356d", margin: "1rem 1rem" }}
                    onClick={handleSuggestEvent}
                >
                    Suggest me some events
                </Button>
            </div>
            {events && (<div className="blog-list">

                <h2
                    style={{ paddingBottom: "16px", paddingLeft: "0.5px" }}
                >{`All suggested events`}</h2>
                {events.map((event) => (
                    <EventPreview event={event} kind={"suggested"} key={event.id} />
                ))}
            </div>)}
            {error && <div>{error}</div>}
        </div>
    );
}

export default EventSuggestion;
