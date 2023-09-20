import { useState } from "react";
import "./CreateEvent.css";
import eventService from "./EventService";
import { useNavigate } from "react-router-dom";
import Alert from "@mui/material/Alert";
import SuggestionWindow from "./SuggestionsWindow";
import zIndex from "@mui/material/styles/zIndex";

const CreateEvent = () => {
  const navigate = useNavigate();
  const [showSuccessAlert, setShowSuccessAlert] = useState(false);
  const [showFailureAlert, setShowFailureAlert] = useState(false);
  const [suggestionData, setSuggestionData] = useState({});
  const [enableSuggestions, setEnableSuggestions] = useState(false);
  const [enteredName, setEnteredName] = useState("");
  const [enteredDate, setEnteredDate] = useState();
  const [enteredMode, setEnteredMode] = useState("0");
  const [enteredModeDetails, setEnteredModeDetails] = useState("");
  const [enteredTopic, setEnteredTopic] = useState("");
  const [enteredSpeakers, setEnteredSpeakers] = useState("");
  const [enteredEventDetails, setEnteredEventDetails] = useState("");
  const [enteredPoc, setEnteredPoc] = useState("");
  const [enteredRules, setEnteredRules] = useState("");
  const [enteredDeadline, setEnteredDeadLine] = useState();
  const [enteredCommunity, setEnteredCommunity] = useState("");
  const [enteredCapacity, setEnteredCapacity] = useState("");
  const [enteredType, setEnteredType] = useState("0");
  const [enteredTags, setEnteredTags] = useState("");

  const updateEnableSuggestion = () => {
    console.log("inside update suggestion");
    console.log(enteredName);
    console.log(enteredTopic);
    console.log(enteredEventDetails);

    if (
      enteredName !== "" &&
      enteredTopic !== "" &&
      enteredEventDetails !== ""
    ) {
      console.log("enabling suggestion");
      setEnableSuggestions(true);
    } else {
      setEnableSuggestions(false);
    }
  };

  const nameChangeHandler = (event) => {
    setEnteredName(event.target.value);
    // console.log(enteredName);
    updateEnableSuggestion();
  };
  const dateChangeHandler = (event) => {
    setEnteredDate(event.target.value);
  };
  const modeChangeHandler = (event) => {
    setEnteredMode(event.target.value);
  };
  const modeDetailsChangeHandler = (event) => {
    setEnteredModeDetails(event.target.value);
  };
  const topicChangeHandler = (event) => {
    setEnteredTopic(event.target.value);
    // console.log(enteredTopic);
    updateEnableSuggestion();
  };
  const speakersChangeHandler = (event) => {
    setEnteredSpeakers(event.target.value);
  };
  const detailsChangeHandler = (event) => {
    setEnteredEventDetails(event.target.value);
    // console.log(enteredTopic);
    updateEnableSuggestion();
  };
  const pocChangeHandler = (event) => {
    setEnteredPoc(event.target.value);
  };
  const rulesChangeHandler = (event) => {
    setEnteredRules(event.target.value);
  };
  const deadlineChangeHandler = (event) => {
    setEnteredDeadLine(event.target.value);
  };
  const communityChangeHandler = (event) => {
    setEnteredCommunity(event.target.value);
  };
  const capacityChangeHandler = (event) => {
    setEnteredCapacity(event.target.value);
  };
  const typeChangeHandler = (event) => {
    setEnteredType(event.target.value);
  };
  const tagsChangeHandler = (event) => {
    setEnteredTags(event.target.value);
  };
  const suggestionsChangeHandler = async () => {
    console.log("clicked");
    const improveTexData = {
      name: enteredName,
      community: enteredTopic,
      Description: enteredEventDetails,
    };
    try {
      const response = await eventService.getSuggestions(improveTexData);
      console.log("response from text suggestion: ");
      console.log(response);
      setSuggestionData(response);
    } catch {
      console.log("text suggestions failed");
    }
  };

  const submitHandler = async (event) => {
    event.preventDefault();

    const expenseData = {
      name: enteredName,
      date: new Date(enteredDate),
      mode: parseInt(enteredMode),
      modelDetails: enteredModeDetails,
      topic: enteredTopic,
      speakers: enteredSpeakers,
      details: enteredEventDetails,
      personOfContact: enteredPoc,
      rules: enteredRules,
      deadline: new Date(enteredDeadline),
      community: enteredCommunity,
      capacity: parseInt(enteredCapacity),
      type: parseInt(enteredType),
      tags: enteredTags,
    };
    console.log(expenseData);

    try {
      const response = await eventService.create([expenseData]);
      console.log("response from create event: ");
      console.log(response);
      setShowSuccessAlert(true);
    } catch {
      console.log("create event request failed");
      setShowFailureAlert(true);
    }

    // setEnteredName("");
    // setEnteredDate("");
    // setEnteredMode("");
    // setEnteredModeDetails("");
    // setEnteredTopic("");
    // setEnteredSpeakers("");
    // setEnteredEventDetails("");
    // setEnteredPoc("");
    // setEnteredEnteredRules("");
    // setEnteredDeadLine("");
    // setEnteredCommunity("");
    // setEnteredCapacity("");
    // setEnteredType("");
    // setEnteredTags("");
  };

  return (
    <div className="create-event">
      <div
        className={showSuccessAlert ? "new-expense backdrop" : "new-expense"}
      >
        <form onSubmit={submitHandler}>
          <div className="new-expense__controls">
            <div className="new-expense__control">
              <label>Name</label>
              <input
                type="text"
                value={enteredName}
                onChange={nameChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Type</label>
              <select
                name="cars"
                id="cars"
                value={enteredType}
                onChange={typeChangeHandler}
                required
              >
                <option value="1">External</option>
                <option value="0">Internal</option>
              </select>
            </div>
            <div className="new-expense__control">
              <label>Date</label>
              <input
                type="datetime-local"
                min="2019-01-01"
                max="2022-12-31"
                value={enteredDate}
                onChange={dateChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Deadline</label>
              <input
                type="datetime-local"
                min="2019-01-01"
                max="2022-12-31"
                value={enteredDeadline}
                onChange={deadlineChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Mode</label>
              <select
                name="cars"
                id="cars"
                value={enteredMode}
                onChange={modeChangeHandler}
                required
              >
                <option value="1">Online</option>
                <option value="0">Offline</option>
              </select>
            </div>
            <div className="new-expense__control">
              <label>Mode Details</label>
              <input
                type="text"
                value={enteredModeDetails}
                onChange={modeDetailsChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Topic</label>
              <input
                type="text"
                value={enteredTopic}
                onChange={topicChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Community</label>
              <input
                type="text"
                value={enteredCommunity}
                onChange={communityChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Speaker</label>
              <input
                type="text"
                value={enteredSpeakers}
                onChange={speakersChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Person Of Contact</label>
              <input
                type="email"
                value={enteredPoc}
                onChange={pocChangeHandler}
                required
              />
            </div>
            <div className="new-expense__control">
              <label>Capacity</label>
              <input
                type="number"
                value={enteredCapacity}
                onChange={capacityChangeHandler}
              />
            </div>
            <div className="new-expense__control">
              <label>Tags</label>
              <input
                type="text"
                value={enteredTags}
                onChange={tagsChangeHandler}
              />
            </div>
            <div className="new-expense__control">
              <label>Event Details</label>
              <textarea
                rows="4"
                cols="45"
                value={enteredEventDetails}
                onChange={detailsChangeHandler}
              />
            </div>
            <div className="new-expense__control">
              <label>Rules</label>
              <textarea
                rows="4"
                cols="45"
                value={enteredRules}
                onChange={rulesChangeHandler}
              />
            </div>
          </div>
          <div className="new-expense__actions">
            <button type="submit" className="button1">
              Create
            </button>
          </div>
        </form>
      </div>
      {showSuccessAlert && (
        <Alert
          className="alert"
          onClose={() => {
            setShowSuccessAlert(false);
            navigate("/events/upcoming");
          }}
          style={{ zIndex: 2 }}
        >
          Event created successfully!
        </Alert>
      )}
      {showFailureAlert && (
        <Alert
          className="alert error"
          onClose={() => {
            setShowFailureAlert(false);
          }}
          severity="error"
        >
          Error occurred while creating the event!
        </Alert>
      )}
      <SuggestionWindow
        data={suggestionData}
        onClick={suggestionsChangeHandler}
        disable={!enableSuggestions}
      ></SuggestionWindow>
    </div>
  );
};

export default CreateEvent;
