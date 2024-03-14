using System.Net.Http.Headers;
using System.Text.Json;

namespace berry.interaction.receivers;

public interface INotificationReceiver
{
    public bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue);

    public Task ReceiveNotification(JsonElement notificationBody);
}