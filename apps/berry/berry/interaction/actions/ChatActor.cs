using leash.chat;
using leash.chat.providers;
using Microsoft.AspNetCore.Mvc;

namespace berry.interaction.actions;

public class ChatActor(IChatProvider chatProvider) : IChatActor
{
    private IChatProvider ChatProvider { get; init; } = chatProvider;

    public Task AnswerChatMessage(IChatSpace space, IChatMessage message, Action<Func<object, OkObjectResult>>? callback = null) =>
        ChatProvider.SendMessageToSpace(space, message, callback);
}