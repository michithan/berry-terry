namespace leash.chat;

public abstract class ChatMessageBase : IChatMessage
{
    public required string Text { get; init; }
}