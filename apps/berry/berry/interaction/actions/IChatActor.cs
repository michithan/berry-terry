using leash.chat;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.actions;

public interface IChatActor
{
    public Task AnswerChatMessage(IChatSpace space, IChatMessage message, Action<Func<object, OkObjectResult>>? callback = null);
}