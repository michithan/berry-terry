using leash.chat;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.handlers;

public interface IChatHandler
{
    public Task HandleChatMessage(IChatSpace space, IChatMessage chatMessage, Action<Func<object, OkObjectResult>>? callback = null);
}