using System.Text.Json;
using berry.interaction.receivers;
using Microsoft.AspNetCore.Mvc;

namespace berry;

[Route("api/webhooks")]
[ApiController]
public class WebHookController : ControllerBase
{
    private AzureDevOpNotificationReceiver AzureDevOpNotificationReceiver { get; init; }

    private GoogleChatNotificationReceiver GoogleChatNotificationReceiver { get; init; }

    public WebHookController(AzureDevOpNotificationReceiver azureDevOpNotificationReceiver, GoogleChatNotificationReceiver googleChatNotificationReceiver)
    {
        Console.WriteLine("WebHookController created");
        AzureDevOpNotificationReceiver = azureDevOpNotificationReceiver;
        GoogleChatNotificationReceiver = googleChatNotificationReceiver;
    }

    [HttpPost("ado")]
    public async Task<IActionResult> HandleAdoPost([FromBody] JsonElement notificationBody)
    {
        Console.WriteLine("Received ADO notification");
        Console.WriteLine(notificationBody.ToString());
        await AzureDevOpNotificationReceiver.ReceiveNotification(notificationBody);
        return Ok();
    }

    [HttpPost("google")]
    public async Task<IActionResult> HandleGooglePost([FromBody] JsonElement notificationBody)
    {
        Console.WriteLine("Received Google notification");
        Console.WriteLine(notificationBody.ToString());
        await GoogleChatNotificationReceiver.ReceiveNotification(notificationBody);
        return Ok();
    }
}
