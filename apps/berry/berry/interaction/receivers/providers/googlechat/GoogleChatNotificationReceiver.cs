using System.Net.Http.Headers;
using System.Text.Json;
using berry.interaction.handlers;
using leash.chat.providers.google;
using leash.utils;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.receivers.providers.googlechat;

public class GoogleChatNotificationReceiver(IGoogleChatProvider googleChatProvider, IChatHandler chatHandler) : NotificationReceiverBase
{
    IGoogleChatProvider GoogleChatProvider { get; init; } = googleChatProvider;

    IChatHandler ChatHandler { get; init; } = chatHandler;

    public override bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue) => true;

    public override Task ReceiveNotification(JsonElement notificationBody, Action<Func<object, OkObjectResult>>? callback = null)
    {
        var eventType = notificationBody.GetPropertyValueOrDefault<string>("type");

        if (eventType != "MESSAGE")
        {
            return Task.CompletedTask;
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

        return ChatHandler.HandleChatMessage(space, message, callback);
    }
}