using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.receivers;

public interface INotificationReceiver
{
    public bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue);

    public Task ReceiveNotification(JsonElement notificationBody, Action<Func<object, OkObjectResult>>? callback = null);
}