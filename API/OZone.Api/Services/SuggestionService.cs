using System.Text;
using System.Text.Json;
using OZone.Api.Constants;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;
using OZone.Api.Models;

namespace OZone.Api.Services;

public interface ISuggestionService
{
    Task<IEnumerable<Event>> SuggestEvent(EventSuggestionRequest req);
    Task<SuggestResponse> SuggestTopic(TopicSuggestionRequest req);
    Task<ImproveEventResponse> ImproveEvent(TextSuggestionRequest req);
}

public class SuggestionService : ISuggestionService
{
    private readonly ILogger<SuggestionService> _logger;
    private readonly IEventService _eventService;
    private readonly IOpenAiIntegration _openAi;

    public SuggestionService(ILogger<SuggestionService> logger, IEventService eventService, IOpenAiIntegration openAi)
    {
        _logger = logger;
        _eventService = eventService;
        _openAi = openAi;
    }

    public async Task<IEnumerable<Event>> SuggestEvent(EventSuggestionRequest req)
    {
        var subs = await _eventService.GetSubscriptionsByEmail(req.Email);
        var events = await _eventService.Get(EventKind.Upcoming);
        string? subscribedEvents = null;
        string? nonSubscribedEvents = null;

        try
        {
            subscribedEvents = string.Join(',', subs.Select(x => x.Event!.Name));
            events = events!.Except(subs.Select(x => x.Event));
            nonSubscribedEvents = string.Join(',', events.Select(x => x.Name));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while parsing event names!");
        }

        StringBuilder prompt = new();
        prompt.Append($"I am part of '{req.Community}' community.");
        var totalSuggestions = 2;

        if (!string.IsNullOrWhiteSpace(subscribedEvents))
        {
            prompt.Append($"I have attended [{subscribedEvents}] events in the past.");
        }

        prompt.Append(@$"which of the following event would you recommend? 
            please select only {totalSuggestions} out of these [{nonSubscribedEvents}].
            The output should be json array with name property.");
        prompt.Append("For e.g. [{'name':'value'},{'name':'value'}]");

        var aiSuggestion = await _openAi.GetAiSuggestion(prompt.ToString());
        // var aiSuggestion = "\n\n[{\"name\":\"EF Core\"},{\"name\":\"DotNet API Design\"}]";

        var suggestedEvents = new List<Event>();
        try
        {
            var serialized = JsonSerializer.Deserialize<List<SuggestedEventFromOpenAi>>(aiSuggestion,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            foreach (var _ in serialized)
            {
                var eventT = await _eventService.GetByName(_.Name);
                suggestedEvents.Add(eventT);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not deserialise the AI response");
            suggestedEvents = (await _eventService.Get(EventKind.Upcoming)).ToList();
        }

        return suggestedEvents;
    }

    public async Task<SuggestResponse> SuggestTopic(TopicSuggestionRequest req)
    {
        var prompt = @$"We are orgnaising an event for people from {req.Community} community, 
        please list some of the interesting topics on which the event can be organised for this community. 
        Output should be the numbered list with very brief description of the topic";

        var aiSuggestion = await _openAi.GetAiSuggestion(prompt);

        return new SuggestResponse { Suggestion = aiSuggestion };
    }

    public async Task<ImproveEventResponse> ImproveEvent(TextSuggestionRequest req)
    {
        var prompt = @$"I am writing a description of an event for people from '{req.Community}' community.
        The name of the event is '{req.Name}'.
        The current description is '{req.Description}'.
        Please suggest better way of writing it, also suggest around 10 topics that can be covered.
        Output should be concise and clear and in below format: \
            improved_event_name:START'put suggested event name here'END!,improved_description:START'put suggested description here'END!,suggested_topics:START'put suggested topics to be covered here'END!";

        var suggestion = await _openAi.GetAiSuggestion(prompt);
//         var suggestion = @"
//
// improved_event_name: STARTUnlock the Power of Unit Testing in C#END!, 
// improved_description: STARTThis event will provide a comprehensive overview of the fundamentals and advanced functions of unit testing in C#. Participants will come away with a better understanding of its vital role in software development.END!,
// suggested_topics: STARTOverview of Unit Testing; Setting Up a Testing Environment; Writing Useful Tests; Customizing Tests; Mocking and Fakes; Debugging Tests; Performance Testing; Automated Testing; Overview of Unit Testing FrameworksEND!";
//         
        ImproveEventResponse res = new();
        try
        {
            var improved_event_name = suggestion.Split("improved_event_name")[1];
            improved_event_name = improved_event_name.Split("improved_description")[0];
            improved_event_name = RemoveTokens(improved_event_name);

            var improved_description = suggestion.Split("improved_description")[1];
            improved_description = improved_description.Split("suggested_topics")[0];
            improved_description = RemoveTokens(improved_description);

            var suggested_topics = suggestion.Split("suggested_topics")[1];
            suggested_topics = RemoveTokens(suggested_topics);

            suggestion = RemoveTokens(suggestion);

            res = new()
            {
                Name = improved_event_name,
                Description = improved_description,
                Topic = suggested_topics,
                FullResponse = suggestion
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing the suggestion.");
            res = new ImproveEventResponse { FullResponse = suggestion };
        }

        return res;
    }

    private string RemoveTokens(string input)
    {
        try
        {
            string[] tokens =
            {
                ": START", "START", "END!,", "END!", "improved_event_name", "improved_description", "suggested_topics"
            };
            foreach (var token in tokens)
            {
                if (input.Contains(token))
                    input = input.Replace(token, null);
            }

            return input;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while remove tokens from suggestion.");
            return input;
        }
    }
}