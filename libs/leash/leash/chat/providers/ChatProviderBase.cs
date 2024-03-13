namespace leash.chat.providers;

public abstract class ChatProviderBase : IChatProvider
{
    public abstract void SendMessageToSpace(IChatMessage message, IChatSpace space);

    public abstract IEnumerable<IChatMessage> GetAllUnreadMessages();
}