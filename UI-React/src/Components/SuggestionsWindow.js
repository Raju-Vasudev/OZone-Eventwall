import "./SuggestionWindow.css";

const SuggestionWindow = (props) => {
  const clickHandler = () => {
    props.onClick();
  };
  return (
    <div className="suggestion-window">
      <p className="info">*Get suggestions for your inputs</p>
      <button
        onClick={clickHandler}
        disabled={props.disable}
        className={props.disable ? "inactive" : ""}
      >
        Improve
      </button>
      <div className="details">
        <p className="title">Event Name </p>
        <p className="description">{props.data.name}</p>
      </div>
      <div className="details">
        <p className="title">Topic </p>
        <p className="description"> {props.data.topic}</p>
      </div>
      <div className="details">
        <p className="title">Event Details </p>
        <p className="description">{props.data.description}</p>
      </div>
      {/* <div className="details">
        <p className="title">Full Response </p>
        <p className="description">{props.data.fullResponse}</p>
      </div> */}
    </div>
  );
};

export default SuggestionWindow;
