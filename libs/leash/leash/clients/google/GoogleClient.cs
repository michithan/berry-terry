using Google.Apis.HangoutsChat.v1.Data;

namespace leash.clients.google;

public class GoogleClient(GoogleClientConfiguration googleClientConfiguration) : IGoogleClient
{
    private GoogleClientCore GoogleClientCore { get; init; } = new GoogleClientCore(googleClientConfiguration);

    public void SendMessageToSpace(Message message, string spaceName)
    {
        GoogleClientCore.HangoutsChatService.Spaces.Messages.Create(message, spaceName).Execute();
    }

    public ListSpacesResponse GetAllSpaces()
    {
        return GoogleClientCore.HangoutsChatService.Spaces.List().Execute();
    }

    public ListMessagesResponse GetAllSpaceMessages(Space space, string? filter = null)
    {
        var query = GoogleClientCore.HangoutsChatService.Spaces.Messages.List(space.Name);
        if (filter != null)
        {
            query.Filter = filter;
        }
        return query.Execute();
    }

    public List<Message> GetAllUnreadMessages() =>
        GetAllSpaces()
            .Spaces
            .SelectMany(space => GetAllSpaceMessages(space, "is:unread").Messages)
            .ToList();
}
