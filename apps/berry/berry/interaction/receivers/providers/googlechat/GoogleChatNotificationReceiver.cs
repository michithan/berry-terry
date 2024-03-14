using System.Net.Http.Headers;
using System.Text.Json;
using berry.interaction.handlers;
using leash.chat.providers.google;
using leash.utils;

namespace berry.interaction.receivers.providers.googlechat;

public class GoogleChatNotificationReceiver(IGoogleChatProvider googleChatProvider, IChatHandler chatHandler) : NotificationReceiverBase
{
    IGoogleChatProvider GoogleChatProvider { get; init; } = googleChatProvider;

    IChatHandler ChatHandler { get; init; } = chatHandler;

    public override bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue) => true;

    public override async Task<string?> ReceiveNotification(JsonElement notificationBody)
    {
        var eventType = notificationBody.GetPropertyValueOrDefault<string>("type");

        if (eventType != "MESSAGE")
        {
            return null;
        }

        var space = new GoogleChatSpace
        {
            Name = notificationBody.GetPropertyValueOrDefault<string>("space", "name"),
        };
        var message = new GoogleChatMessage
        {
            Text = notificationBody.GetPropertyValueOrDefault<string>("message", "text"),
            IsBotMentioned = false,
        };

        return await ChatHandler.HandleChatMessage(space, message);
    }
}