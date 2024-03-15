using System.Net.Http.Headers;
using System.Text.Json;
using berry.interaction.receivers.providers.azuredevops;
using berry.interaction.receivers.providers.googlechat;
using leash.chat.providers.google;
using leash.utils;
using Microsoft.AspNetCore.Mvc;

namespace berry;

[Route("api/webhooks")]
[ApiController]
public class WebHookController(ILogger<WebHookController> logger, AzureDevOpNotificationReceiver azureDevOpNotificationReceiver, GoogleChatNotificationReceiver googleChatNotificationReceiver) : ControllerBase
{
    private ILogger Logger { get; init; } = logger;

    private AzureDevOpNotificationReceiver AzureDevOpNotificationReceiver { get; init; } = azureDevOpNotificationReceiver;

    private GoogleChatNotificationReceiver GoogleChatNotificationReceiver { get; init; } = googleChatNotificationReceiver;

    [HttpPost("ado")]
    public IActionResult HandleAdoPost([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] JsonElement notificationBody)
    {
        var authorizationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
        if (AzureDevOpNotificationReceiver.IsAuthorized(authorizationHeaderValue) is false)
        {
            return Unauthorized();
        }

        Logger.LogInformation("Received ADO notification");
        Logger.LogInformation(notificationBody.ToBeautifulJsonString());
        AzureDevOpNotificationReceiver.ReceiveNotification(notificationBody);

        return Ok();
    }

    [HttpPost("google")]
    public async Task<ActionResult<GoogleChatMessageResponse>> HandleGooglePost([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] JsonElement notificationBody)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        var authorizationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
        if (GoogleChatNotificationReceiver.IsAuthorized(authorizationHeaderValue) is false)
        {
            return Unauthorized();
        }

        Logger.LogInformation("Received google notification");
        Logger.LogInformation(notificationBody.ToBeautifulJsonString());
        string threadName = notificationBody.GetPropertyValueOrDefault<string>("message", "thread", "name");

        var taskCompletionSource = new TaskCompletionSource<OkObjectResult>();

        _ = GoogleChatNotificationReceiver.ReceiveNotification(notificationBody, callback => taskCompletionSource.SetResult(callback(threadName)));

        var result = await taskCompletionSource.Task;

        watch.Stop();
        Logger.LogInformation($"Processed google chat notification in {watch.ElapsedMilliseconds}ms");

        return result;
    }
}
