using System.Text;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;

namespace OZone.Api.Services;

public interface IEventNotificationService
{
    Task SendSubscriptionNotifications(Event createEvent, string to);
    Task SendEventNotifications(Event createEvent, string to);
    Task SendReminderNotifications(Event createEvent, string to);
}

public class EventNotificationService : IEventNotificationService
{
    private readonly ILogger<EventNotificationService> _logger;
    private readonly IEmailSender _emailSender;

    public EventNotificationService(ILogger<EventNotificationService> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    public async Task SendSubscriptionNotifications(Event createEvent, string to)
    {
        try
        {
            var subject = $"Subscription to '{createEvent.Name}' event is successful";
            await _emailSender.Send(to, subject, CreateEventTemplate(createEvent));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }

    public async Task SendEventNotifications(Event createEvent, string to)
    {
        try
        {
            var subject = $"A new event '{createEvent.Name}' is registered";
            await _emailSender.Send(to, subject, CreateEventTemplate(createEvent));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }
    
    public async Task SendReminderNotifications(Event createEvent, string to)
    {
        try
        {
            var subject = $"Reminder for '{createEvent.Name}' event";
            await _emailSender.Send(to, subject, CreateEventTemplate(createEvent));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError(ex, "Could not send email notification!");
        }
    }

    public string CreateEventTemplate(Event createEvent)
    {
        StringBuilder body = new StringBuilder();
        
        body.AppendLine("<h3>Event details:</h3>");
        body.AppendLine($"<b>Name</b>: {createEvent.Name}");
        body.AppendLine($"<br/><b>Date</b>: {createEvent.Date}");
        body.AppendLine($"<br/><b>Speakers</b>: {createEvent.Speakers}");
        body.AppendLine($"<br/><b>Mode</b>: {createEvent.Mode.ToString()}");
        body.AppendLine($"<br/><b>Mode Details</b>: {createEvent.ModelDetails}");
        body.AppendLine($"<br/><b>Topics</b>: {createEvent.Topic}");
        body.AppendLine($"<br/><b>Details</b>: {createEvent.Details}");
        body.AppendLine($"<br/><b>Person of contact</b>: {createEvent.PersonOfContact}");
        
        body.AppendLine("<h4>More details:</h4>");
        body.AppendLine($"<b>Rules</b>: {createEvent.Rules}");
        body.AppendLine($"<br/><b>Deadline</b>: {createEvent.Deadline}");
        body.AppendLine($"<br/><b>Community</b>: {createEvent.Community}");
        body.AppendLine($"<br/><b>Capacity</b>: {createEvent.Capacity}");
        body.AppendLine($"<br/><b>Type</b>: {createEvent.Type.ToString()}");
        body.AppendLine($"<br/><b>Tags</b>: {createEvent.Tags}");

        return body.ToString();
    }
}