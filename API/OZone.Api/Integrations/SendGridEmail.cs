using SendGrid;
using SendGrid.Helpers.Mail;

namespace OZone.Api.Integrations;

public interface IEmailSender
{
    Task Send(string to, string subject, string body);
}

public class SendGridEmail : IEmailSender
{
    private readonly ILogger<SendGridEmail> _logger;
    private readonly string _apiKey;
    private readonly string _from;

    public SendGridEmail(ILogger<SendGridEmail> logger, IConfiguration config)
    {
        _logger = logger;
        _apiKey = config.GetValue<string>("Email:Key")!;
        _from = config.GetValue<string>("Email:From")!;
    }

    public async Task Send(string to, string subject, string body)
    {
        var client = new SendGridClient(_apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(_from, "OZone Team"),
            Subject = subject,
            HtmlContent = body
        };
        msg.AddTo(new EmailAddress(to));
        var response = await client.SendEmailAsync(msg);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Email sent successfully. From:{from}, to:{to}, subject:{subject}", _from, to,
                subject);
        }
        else
        {
            var content =await response.DeserializeResponseBodyAsync();
            _logger.LogError("Email sending failed!. From:{from}, to:{to}, subject:{subject}, reason:{content}", _from, to,
                subject,content);
            throw new ApplicationException("Error occurred while sending email!");
        }
        
        
    }
}