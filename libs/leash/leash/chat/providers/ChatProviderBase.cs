using Microsoft.AspNetCore.Mvc;

namespace leash.chat.providers;

public abstract class ChatProviderBase : IChatProvider
{
    public abstract Task SendMessageToSpace(IChatSpace space, IChatMessage message, Action<Func<object, OkObjectResult>>? callback = null);

    public abstract IEnumerable<IChatMessage> GetAllUnreadMessages();
}