namespace leash.chat.providers;

public interface IChatProvider
{
    void SendMessageToSpace(IChatMessage message, IChatSpace space);

    public IEnumerable<IChatMessage> GetAllUnreadMessages();
}