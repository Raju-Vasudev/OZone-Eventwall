using OZone.Api.Domain.Constants;

namespace OZone.Api.Domain.Models;

public class Event
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public DateTime Date { get; set; }
    public EMode Mode { get; set; }
    public string ModelDetails { get; set; }= default!;
    public string Topic { get; set; }= default!;
    public string Speakers { get; set; }= default!;
    public string Details { get; set; }= default!;
    public string PersonOfContact { get; set; }= default!;
    public string Rules { get; set; }= default!;
    public DateTime Deadline { get; set; }
    public string Community { get; set; }= default!;
    public int Capacity { get; set; }
    public EType Type { get; set; }
    public string Tags { get; set; }= default!;

    public List<Subscription> Subscriptions { get; set; } = new();
}