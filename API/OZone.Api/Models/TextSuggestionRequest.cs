namespace OZone.Api.Models;

public class TextSuggestionRequest
{
    public string Name { get; set; }= default!;
    public string Community { get; set; } = default!;
    public string Description { get; set; }= default!;
}