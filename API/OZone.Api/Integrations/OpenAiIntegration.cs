using System.Text;
using System.Text.Json;

namespace OZone.Api.Integrations;

public interface IOpenAiIntegration
{
    Task<string> GetAiSuggestion(string? question);
}

public class OpenAiIntegration : IOpenAiIntegration
{
    private readonly ILogger<OpenAiIntegration> _logger;
    private readonly string _apiKey;

    public OpenAiIntegration(ILogger<OpenAiIntegration> logger, IConfiguration config)
    {
        _logger = logger;
        _apiKey = config.GetValue<string>("OpenAI:Key")!;
    }

    public async Task<string> GetAiSuggestion(string? question)
    {
        var prompt = $"{question}";

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

        var request = new HttpRequestMessage(HttpMethod.Post, $"https://api.openai.com/v1/completions");
        request.Content = new StringContent(JsonSerializer.Serialize(
            new OpenAI_Request
            {
                model = "text-davinci-003",
                prompt = prompt,
                temperature = 1,
                max_tokens = 128,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0
            }), Encoding.UTF8, "application/json");

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<OpenAI_Response>(responseString);

        var suggestion= responseJson?.choices?[0].text!;
        
        _logger.LogInformation("Suggestion from Open AI:{suggestion}",suggestion);
        return suggestion;
    }
}

class OpenAI_Request
{
    public string? model { get; set; }
    public string? prompt { get; set; }
    public double? temperature { get; set; }
    public int? max_tokens { get; set; }
    public double? top_p { get; set; }
    public double? frequency_penalty { get; set; }
    public double? presence_penalty { get; set; }
}

class Choice
{
    public string? text { get; set; }
}

class OpenAI_Response
{
    public Choice[]? choices { get; set; }
}