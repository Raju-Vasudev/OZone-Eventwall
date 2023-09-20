using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Models;
using OZone.Api.Services;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ILogger<SubscriptionsController> _logger;
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ILogger<SubscriptionsController> logger,
        ISubscriptionService subscriptionService)
    {
        _logger = logger;
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _subscriptionService.Get());
    }

    /// <summary>
    /// Subscribe a user to an event
    /// </summary>
    /// <param name="req">Subscription details</param>
    /// <returns>Created subscription</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Subscribe(CreateSubscriptionRequest req)
    {
        try
        {
            var sub = await _subscriptionService.Create(req);
            return Ok(new CreateSubscriptionResponse
            {
                Id = sub.Id,
                EventId = sub.EventId,
                UserId = sub.UserId
            });
        }
        catch (ApplicationException e)
        {
            return BadRequest(new { Error = e.Message });
        }
    }
}