using Google.Apis.HangoutsChat.v1.Data;

namespace leash.clients.google;

public class GoogleClient(GoogleClientConfiguration googleClientConfiguration) : IGoogleClient
{
    private GoogleClientCore GoogleClientCore { get; init; } = new GoogleClientCore(googleClientConfiguration);

    public void SendMessage(Message message, string spaceName)
    {
        GoogleClientCore.HangoutsChatService.Spaces.Messages.Create(message, spaceName).Execute();
    }
}