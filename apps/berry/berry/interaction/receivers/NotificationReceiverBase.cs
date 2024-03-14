using System.Net.Http.Headers;
using System.Text.Json;

namespace berry.interaction.receivers;

public abstract class NotificationReceiverBase : INotificationReceiver
{
    public abstract bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue);

    public abstract Task<string?> ReceiveNotification(JsonElement notificationBody);
}