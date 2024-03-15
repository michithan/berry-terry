using leash.clients.google;
using leash.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace leash.chat.providers.google;

public class GoogleChatProvider(ILogger<GoogleChatProvider> logger, IGoogleClient googleClient) : ChatProviderBase, IGoogleChatProvider
{
    private IGoogleClient GoogleClient { get; init; } = googleClient;

    private ILogger Logger { get; init; } = logger;

    public override Task SendMessageToSpace(IChatSpace space, IChatMessage message, Action<Func<object, OkObjectResult>>? callback = null)
    {
        callback?.Invoke(data =>
        {
            GoogleChatMessageResponse response = new()
            {
                text = message.Text,
                thread = new()
                {
                    name = (string)data
                }
            };
            Logger.LogInformation("Responded to google chat message");
            Logger.LogInformation(response.ToBeautifulJsonString());
            return new OkObjectResult(response);
        });
        return Task.CompletedTask;
    }

    public override IEnumerable<IChatMessage> GetAllUnreadMessages()
    {
        var unreadMessages = GoogleClient.GetAllUnreadMessages();
        return unreadMessages.Select(message => message.ToChatMessage());
    }
}