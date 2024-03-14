namespace leash.chat.providers;

public abstract class ChatProviderBase : IChatProvider
{
    public abstract Task SendMessageToSpace(IChatSpace space, IChatMessage message);

    public abstract IEnumerable<IChatMessage> GetAllUnreadMessages();
}