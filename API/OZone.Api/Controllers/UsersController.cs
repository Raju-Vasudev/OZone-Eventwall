using Microsoft.AspNetCore.Mvc;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly EventContext _db;

    public UsersController(ILogger<UsersController> logger, EventContext db)
    {
        _logger = logger;
        _db = db;
    }
    
    [HttpGet]
    public IEnumerable<User> Get()
    {
        return _db.Users.ToList();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public User Subscribe(User user)
    {
        var eventT = _db.Users.Add(user);
        _db.SaveChanges();

        return eventT.Entity;
    }
}