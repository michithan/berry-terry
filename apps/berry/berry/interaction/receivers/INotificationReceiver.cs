using System.Text.Json;

namespace berry.interaction.receivers;

public interface INotificationReceiverBase
{
    public Task<string?> ReceiveNotification(JsonElement notificationBody);
}