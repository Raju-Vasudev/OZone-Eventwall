namespace OZone.Api.Models;

public class MyRateLimitOptions
{
    public int PermitLimit { get; set; }
    public int Window { get; set; }
    public int QueueLimit { get; set; }
}