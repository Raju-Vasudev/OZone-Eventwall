using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using OZone.Api.Constants;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;
using OZone.Api.Models;
using OZone.Api.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
[EnableRateLimiting("fixed")]
public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IEventService _eventService;
    private readonly ISuggestionService _suggestionService;

    public EventsController(ILogger<EventsController> logger, IEventService eventService,
        ISuggestionService suggestionService)
    {
        _logger = logger;
        _eventService = eventService;
        _suggestionService = suggestionService;
    }

    /// <summary>
    /// Get events by kind
    /// </summary>
    /// <param name="kind">Kind of events to return. 'upcoming', 'past'.</param>
    /// <returns>Returns filtered events if kind is provided. Otherwise returns all events.</returns>
    [HttpGet]
    [EnableRateLimiting("fixed")]
    public async Task<IActionResult> Get(string? kind)
    {
        return Ok(await _eventService.Get(kind));
    }

    /// <summary>
    /// Get event by id
    /// </summary>
    /// <param name="id">Unique Guid of the event</param>
    /// <returns>Event details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _eventService.GetById(id));
    }

    /// <summary>
    /// Create an event
    /// </summary>
    /// <param name="createEvent">Event details</param>
    /// <returns>Newly created event</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(IEnumerable<Event> createEvent)
    {
        var events = new List<Event>();
        foreach (var _ in createEvent)
        {
            events.Add(await _eventService.Create(_));
        }

        return Ok(EventResponse.Map(events));
    }

    [HttpPost("suggest")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EnableRateLimiting("openAi")]
    public async Task<IActionResult> SuggestEvent(EventSuggestionRequest req)
    {
        var events = await _suggestionService.SuggestEvent(req);
        return Ok(EventResponse.Map(events));
    }

    [HttpPost("suggest/topic")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EnableRateLimiting("openAi")]
    public async Task<IActionResult> SuggestTopic(TopicSuggestionRequest req)
    {
        return Ok(await _suggestionService.SuggestTopic(req));
    }

    [HttpPost("improve")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [EnableRateLimiting("openAi")]
    public async Task<IActionResult> ImproveText(TextSuggestionRequest req)
    {
        return Ok(await _suggestionService.ImproveEvent(req));
    }
}