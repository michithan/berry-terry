namespace leash.chat.providers;

public interface IChatProvider
{
    Task SendMessageToSpace(IChatSpace space, IChatMessage message);

    public IEnumerable<IChatMessage> GetAllUnreadMessages();
}