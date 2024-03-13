using System.Text.Json;
using leash.chat.providers.google;

namespace berry.interaction.receivers;

public class GoogleChatNotificationReceiver(IGoogleChatProvider googleChatProvider) : NotificationReceiverBase
{
    IGoogleChatProvider GoogleChatProvider { get; init; } = googleChatProvider;

    public override Task ReceiveNotification(JsonElement notificationBody)
    {
        var unreadMessages = GoogleChatProvider.GetAllUnreadMessages();

        var notificationMessage = notificationBody.GetPropertyValueOrDefault<string>("message");
        var notificationSpace = notificationBody.GetPropertyValueOrDefault<string>("space");

        throw new NotImplementedException();
    }
}