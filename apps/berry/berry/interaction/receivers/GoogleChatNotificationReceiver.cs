using System.Text.Json;

namespace berry.interaction.receivers;

public class GoogleChatNotificationReceiver : NotificationReceiverBase
{
    public override Task ReceiveNotification(JsonElement notificationBody)
    {
        throw new NotImplementedException();
    }
}