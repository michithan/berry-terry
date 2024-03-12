using System.Text.Json;
using berry.interaction.receivers;
using Microsoft.AspNetCore.Mvc;

namespace berry;

[Route("api/webhooks")]
[ApiController]
public class WebHookController : ControllerBase
{
    private AzureDevOpNotificationReceiver AzureDevOpWebHookHandler { get; init; }

    public WebHookController(AzureDevOpNotificationReceiver azureDevOpWebHookHandler)
    {
        Console.WriteLine("WebHookController created");
        AzureDevOpWebHookHandler = azureDevOpWebHookHandler;
    }

    [HttpPost("ado")]
    public async Task<IActionResult> HandleAdoPost([FromBody] JsonElement  notificationBody)
    {
        Console.WriteLine("Received ADO notification");
        Console.WriteLine(notificationBody.ToString());
        await AzureDevOpWebHookHandler.ReceiveNotification(notificationBody);
        return Ok();
    }
}
