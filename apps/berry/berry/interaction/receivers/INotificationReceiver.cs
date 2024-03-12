using System.Text.Json;

namespace berry.interaction.receivers;

public interface INotificationReceiverBase
{
    public Task ReceiveNotification(JsonElement notificationBody);
}