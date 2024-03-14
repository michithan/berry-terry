using System.Text.Json;
using berry.interaction.receivers;
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
    public async Task<IActionResult> HandleAdoPost([FromBody] JsonElement notificationBody)
    {
        Logger.LogInformation("Received ADO notification");
        Logger.LogInformation(notificationBody.ToJsonString());
        await AzureDevOpNotificationReceiver.ReceiveNotification(notificationBody);
        return Ok();
    }

    [HttpPost("google")]
    public async Task<ActionResult<GoogleChatMessageResponse>> HandleGooglePost([FromBody] JsonElement notificationBody)
    {
        Logger.LogInformation("Received google notification");
        Logger.LogInformation(notificationBody.ToJsonString());
        var answer = await GoogleChatNotificationReceiver.ReceiveNotification(notificationBody);

        if (answer == null)
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
