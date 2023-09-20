namespace OZone.Api.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; }= default!;

    public List<Subscription> Subscriptions { get; set; } = new();
}