using Microsoft.EntityFrameworkCore;
using OZone.Api.Constants;
using OZone.Api.Domain;

namespace OZone.Api.Services;

public class TimedHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedHostedService> _logger;
    private readonly IServiceProvider _services;
    private Timer? _timer = null;

    public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider services)
    {
        _logger = logger;
        _services = services;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(60));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        using (var scope = _services.CreateScope())
        {
            var notificationService =
                scope.ServiceProvider
                    .GetRequiredService<IEventNotificationService>();
            var eventService =
                scope.ServiceProvider
                    .GetRequiredService<IEventService>();
            var db =
                scope.ServiceProvider
                    .GetRequiredService<EventContext>();

            var events = db.Events
                .Where(x => x.Date.CompareTo(DateTime.UtcNow) > 0)
                .OrderBy(x => x.Date)
                .Include(x => x.Subscriptions).ThenInclude(x => x.User)
                .ToList();

            var tasks = new List<Task>();
            foreach (var subscription in events.SelectMany(x => x.Subscriptions))
            {
                tasks.Add(notificationService.SendEventNotifications(subscription.Event, subscription.User.Email));
            }

            Task.WaitAll(tasks.ToArray());
        }

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}