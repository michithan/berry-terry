namespace leash.chat;

public abstract class ChatSpaceBase : IChatSpace
{
    public required string Name { get; init; }
}