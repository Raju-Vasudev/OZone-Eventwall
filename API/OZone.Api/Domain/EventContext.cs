using Microsoft.EntityFrameworkCore;
using OZone.Api.Domain.Models;

namespace OZone.Api.Domain;

public class EventContext : DbContext
{
    public EventContext(DbContextOptions<EventContext> options)
        : base(options)
    {
    }
    
    public DbSet<Event> Events { get; set; }= null!;
    public DbSet<User> Users { get; set; }= null!;
    public DbSet<Subscription> Subscriptions { get; set; }
}