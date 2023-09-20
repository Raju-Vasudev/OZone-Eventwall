namespace OZone.Api.Models;

public class CreateSubscriptionRequest
{
    public Guid EventId { get; set; }
    public string Email { get; set; }= default!;
    public string Name { get; set; } = default!;
}