using leash.clients.google;

namespace leash.chat.providers.google;

public class GoogleChatProvider(IGoogleClient googleClient) : ChatProviderBase, IGoogleChatProvider
{
    private IGoogleClient GoogleClient { get; init; } = googleClient;

    public override string? SendMessageToSpace(IChatSpace space, IChatMessage message)
    {
        return message.Text;
        // var googleMessage = message.ToGoogleChatMessage();
        // GoogleClient.SendMessageToSpace(googleMessage, space.Name);
    }

    public override IEnumerable<IChatMessage> GetAllUnreadMessages()
    {
        var unreadMessages = GoogleClient.GetAllUnreadMessages();
        return unreadMessages.Select(message => message.ToChatMessage());
    }
}