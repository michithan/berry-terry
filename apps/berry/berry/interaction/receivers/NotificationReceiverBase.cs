using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.receivers;

public abstract class NotificationReceiverBase : INotificationReceiver
{
    public abstract bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue);

    public abstract Task ReceiveNotification(JsonElement notificationBody, Action<Func<object, OkObjectResult>>? callback = null);
}