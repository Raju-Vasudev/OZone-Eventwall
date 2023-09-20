using OZone.Api.Domain.Constants;

namespace OZone.Api.Domain.Models;

public class EventResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = default!;
    public DateTime Date { get; set; }
    public EMode Mode { get; set; }
    public string ModelDetails { get; set; } = default!;
    public string Topic { get; set; } = default!;
    public string Speakers { get; set; } = default!;
    public string Details { get; set; } = default!;
    public string PersonOfContact { get; set; } = default!;
    public string Rules { get; set; } = default!;
    public DateTime Deadline { get; set; }
    public string Community { get; set; } = default!;
    public int Capacity { get; set; }
    public EType Type { get; set; }
    public string Tags { get; set; } = default!;

    public static IEnumerable<EventResponse> Map(IEnumerable<Event> source)
    {
        var dtos = new List<EventResponse>();
        foreach (var item in source)
        {
            dtos.Add(Map(item));
        }

        return dtos;
    }

    public static EventResponse Map(Event source)
    {
        return new EventResponse
        {
            Id = source.Id,
            Name = source.Name,
            Date = source.Date,
            Mode = source.Mode,
            ModelDetails = source.ModelDetails,
            Topic = source.Topic,
            Speakers = source.Speakers,
            Details = source.Details,
            PersonOfContact = source.PersonOfContact,
            Rules = source.Rules,
            Deadline = source.Deadline,
            Community = source.Community,
            Capacity = source.Capacity,
            Type = source.Type,
            Tags = source.Tags
        };
    }
}