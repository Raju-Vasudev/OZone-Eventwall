
using AutoFixture;
using AutoFixture.AutoMoq;
using Bogus;
using OZone.Api.Constants;

namespace OZone.Api.UnitTests;

public class EventServiceTests
{
  private  readonly EventContext dbContext;
  private   EventService eventService;
  private readonly IFixture _fixture;

    public EventServiceTests()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        
        var dbContextOptions = new DbContextOptionsBuilder<EventContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var loggerMock = new Mock<ILogger<EventService>>();
        var emailSenderMock = new Mock<IEventNotificationService>();
   
        dbContext = new EventContext(dbContextOptions);
        eventService = new EventService(loggerMock.Object, dbContext, emailSenderMock.Object);
    }
    
    [Fact]
    public async Task GetById_ValidId_ReturnsEvent()
    {
        // Arrange
        var eventEntity = _fixture.Build<Event>().Without(x=>x.Subscriptions).Create();
    
       dbContext.Events.Add(eventEntity);
       dbContext.SaveChanges();

       // Act
        var result = await eventService.GetById(eventEntity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(eventEntity.Id, result.Id);
        Assert.Equal(eventEntity.Name, result.Name);
    }
    
    [Fact]
    public async Task Get_NoKindSpecified_ReturnsAllEvents()
    {
        // Arrange
        var data = _fixture.Build<Event>().Without(x=>x.Subscriptions).CreateMany(10);

        dbContext.Events.AddRange(data);
        dbContext.SaveChanges();
        
        // Act
        var result = await eventService.Get(null);

        // Assert
        Assert.Equal(data.Count(), result.Count());
    }
        
    [Fact]
    public async Task Get_WhenPastKindSpecified_ReturnsPastEvents()
    {
        // Arrange

        var past = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(-10))
            .CreateMany(10);
        
        var future = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(10))
            .CreateMany(5);
        
        dbContext.Events.AddRange(past);
        dbContext.Events.AddRange(future);
        dbContext.SaveChanges();
        
        // Act
        var result = await eventService.Get(EventKind.Past);

        // Assert
        Assert.Equal(past.Count(), result.Count());
    }
        
    [Fact]
    public async Task Get_WhenUpcomingKindSpecified_ReturnsFutureEvents()
    {
        // Arrange

        var past = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(-10))
            .CreateMany(5);
        
        var future = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(10))
            .CreateMany(10);
        
        dbContext.Events.AddRange(past);
        dbContext.Events.AddRange(future);
        dbContext.SaveChanges();
        
        // Act
        var result = await eventService.Get(EventKind.Upcoming);

        // Assert
        Assert.Equal(future.Count(), result.Count());
    }
        
    [Fact]
    public async Task Get_WhenArchievedKindSpecified_ReturnsAllPastEvents()
    {
        // Arrange
        var past = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(-10))
            .CreateMany(10);
        
        var future = _fixture.Build<Event>()
            .Without(x=>x.Subscriptions)
            .With(x=>x.Date, () => DateTime.UtcNow.AddDays(10))
            .CreateMany(5);
        
        dbContext.Events.AddRange(past);
        dbContext.Events.AddRange(future);
        dbContext.SaveChanges();
        
        // Act
        var result = await eventService.Get(EventKind.Archived);

        // Assert
        Assert.Equal(past.Count(), result.Count());
    }
    
    
}