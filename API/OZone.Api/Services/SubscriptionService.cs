using Microsoft.EntityFrameworkCore;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Models;

namespace OZone.Api.Services;

public interface ISubscriptionService
{
    Task<Subscription> Create(CreateSubscriptionRequest req);
    Task<IEnumerable<Subscription>> Get();
}

public class SubscriptionService : ISubscriptionService
{
    private readonly ILogger<SubscriptionService> _logger;
    private readonly EventContext _db;
    private readonly IEventNotificationService _notificationService;

    public SubscriptionService(ILogger<SubscriptionService> logger, EventContext db,
        IEventNotificationService notificationService)
    {
        _logger = logger;
        _db = db;
        _notificationService = notificationService;
    }

    public async Task<Subscription> Create(CreateSubscriptionRequest req)
    {
        var user = await _db.Users.Where(x => x.Email == req.Email).FirstOrDefaultAsync();

        if (user == null)
        {
            var entity = await _db.Users.AddAsync(new User { Name = req.Name, Email = req.Email });
            user = entity.Entity;
        }

        if (await _db.Subscriptions.AnyAsync(x => x.UserId == user.Id && x.EventId == req.EventId))
        {
            _logger.LogError("Already subscribed to the event!");
            throw new ApplicationException("Already subscribed to the event!");
        }

        var sub = new Subscription
        {
            UserId = user.Id,
            EventId = req.EventId
        };
        var eventFromDb = await _db.Events.Include(x => x.Subscriptions)
            .FirstOrDefaultAsync(x => x.Id == req.EventId);

        ValidateEvent(eventFromDb);

        var createdSub = await _db.Subscriptions.AddAsync(sub);
        await _db.SaveChangesAsync();

        await _notificationService.SendSubscriptionNotifications(eventFromDb!, req.Email);

        return createdSub.Entity;
    }

    public async Task<IEnumerable<Subscription>> Get()
    {
        return await _db.Subscriptions.ToListAsync();
    }

    private void ValidateEvent(Event? eventFromDb)
    {
        if (eventFromDb == null)
        {
            _logger.LogError("Event does not exist!");
            throw new ApplicationException("Event does not exist!");
        }

        if (eventFromDb.Deadline.CompareTo(DateTime.UtcNow) < 0)
        {
            _logger.LogError("Event subscription is closed!");
            throw new ApplicationException("Event subscription is closed!");
        }

        if (eventFromDb.Capacity - eventFromDb.Subscriptions.Count <= 0)
        {
            _logger.LogError("Event subscription is full!");
            throw new ApplicationException("Event subscription is full!");
        }
    }
}