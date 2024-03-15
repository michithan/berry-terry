using Microsoft.AspNetCore.Mvc;

namespace leash.chat.providers;

public interface IChatProvider
{
    Task SendMessageToSpace(IChatSpace space, IChatMessage message, Action<Func<object, OkObjectResult>>? callback = null);

    public IEnumerable<IChatMessage> GetAllUnreadMessages();
}