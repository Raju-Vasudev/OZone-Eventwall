namespace OZone.Api.Models;

public class ImproveEventResponse
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Topic { get; set; }
    public string FullResponse { get; set; } = default!;
}