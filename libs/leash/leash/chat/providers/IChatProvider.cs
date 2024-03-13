namespace leash.chat.providers;

public interface IChatProvider
{
    string? SendMessageToSpace(IChatSpace space, IChatMessage message);

    public IEnumerable<IChatMessage> GetAllUnreadMessages();
}