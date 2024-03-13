using leash.chat;
using leash.chat.providers;

namespace berry.interaction.actions;

public class ChatActor(IChatProvider chatProvider) : IChatActor
{
    private IChatProvider ChatProvider { get; init; } = chatProvider;

    public Task<string?> AnswerChatMessage(IChatSpace space, IChatMessage message) =>
        Task.FromResult(ChatProvider.SendMessageToSpace(space, message));
}