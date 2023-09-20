namespace OZone.Api.Models;

public class CreateSubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
}