namespace OZone.Api.Domain.Models;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public Event? Event { get; set; }
    public User? User { get; set; }
}