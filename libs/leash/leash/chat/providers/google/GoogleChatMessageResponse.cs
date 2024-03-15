namespace leash.chat.providers.google;

public class GoogleChatMessageResponse
{
    public required string text { get; init; }

    public required GoogleChatMessageResponseThread thread { get; init; }
}