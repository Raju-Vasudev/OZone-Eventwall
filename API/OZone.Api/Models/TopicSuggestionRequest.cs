namespace OZone.Api.Models;

public class TopicSuggestionRequest
{
    public string Community { get; set; } = default!;
}
public class EventSuggestionRequest
{
    public string Email { get; set; } = default!;
    public string Community { get; set; } = default!;
}