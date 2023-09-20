using Microsoft.EntityFrameworkCore;
using OZone.Api.Constants;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;

namespace OZone.Api.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> Get(string? kind);
    Task<Event?> GetById(Guid id);
    Task<Event?> GetByName(string name);
    Task<Event> Create(Event createEvent);
    Task<IEnumerable<Subscription>> GetSubscriptionsByEmail(string email);
}

public class EventService : IEventService
{
    private readonly ILogger<EventService> _logger;
    private readonly EventContext _db;
    private readonly IEventNotificationService _emailSender;

    public EventService(ILogger<EventService> logger, EventContext db, IEventNotificationService emailSender)
    {
        _logger = logger;
        _db = db;
        _emailSender = emailSender;
    }

    public async Task<IEnumerable<Event>> Get(string? kind)
    {
        if (kind == EventKind.Upcoming)
            return await _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) > 0).OrderBy(x => x.Date)
                .ToListAsync();

        if (kind == EventKind.Past)
            return await _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) < 0).Take(20)
                .OrderByDescending(x => x.Date).ToListAsync();

        if (kind == EventKind.Archived)
            return await _db.Events.Where(x => x.Date.CompareTo(DateTime.UtcNow) < 0).OrderByDescending(x => x.Date)
                .ToListAsync();

        return _db.Events.ToList();
    }

    public async Task<Event?> GetById(Guid id)
    {
        return await _db.Events.FindAsync(id);
    }

    public Task<Event?> GetByName(string name)
    {
        var eventT = _db.Events
            .AsEnumerable()
            .FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        return Task.FromResult(eventT);
    }

    public async Task<Event> Create(Event createEvent)
    {
        var eventT = _db.Events.Add(createEvent);
        await _db.SaveChangesAsync();

        await _emailSender.SendEventNotifications(createEvent, createEvent.PersonOfContact);
        await _emailSender.SendEventNotifications(createEvent, createEvent.Community);
        return eventT.Entity;
    }

    public async Task<IEnumerable<Subscription>> GetSubscriptionsByEmail(string email)
    {
        var subs = _db.Subscriptions
            .Include(x => x.User)
            .Include(x => x.Event)
            .Where(x => x.User!.Email == email);

        return await subs.ToListAsync();
    }
}