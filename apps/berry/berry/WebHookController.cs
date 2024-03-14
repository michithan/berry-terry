using System.Net.Http.Headers;
using System.Text.Json;
using berry.interaction.receivers.providers.azuredevops;
using berry.interaction.receivers.providers.googlechat;
using Json.More;
using leash.chat.providers.google;
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
        Logger.LogInformation(notificationBody.ToJsonString());
        AzureDevOpNotificationReceiver.ReceiveNotification(notificationBody);

        return Ok();
    }

    [HttpPost("google")]
    public async Task<ActionResult<GoogleChatMessageResponse>> HandleGooglePost([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] JsonElement notificationBody)
    {
        var authorizationHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
        if (GoogleChatNotificationReceiver.IsAuthorized(authorizationHeaderValue) is false)
        {
            return Unauthorized();
        }

        Logger.LogInformation("Received google notification");
        Logger.LogInformation(notificationBody.ToJsonString());

        var answer = await GoogleChatNotificationReceiver.ReceiveNotification(notificationBody);
        if (answer is null)
        {
            return Ok();
        }

        GoogleChatMessageResponse response = new()
        {
            Text = answer,
        };
        return Ok(response);
    }
}
