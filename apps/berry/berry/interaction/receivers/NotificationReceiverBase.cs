using System.Text.Json;

namespace berry.interaction.receivers;

public abstract class NotificationReceiverBase : INotificationReceiverBase
{
    public abstract Task ReceiveNotification(JsonElement notificationBody);
}