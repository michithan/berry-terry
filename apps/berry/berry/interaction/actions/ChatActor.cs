using leash.chat;
using leash.chat.providers;

namespace berry.interaction.actions;

public class ChatActor(IChatProvider chatProvider) : IChatActor
{
    private IChatProvider ChatProvider { get; init; } = chatProvider;

    public Task AnswerChatMessage(IChatSpace space, IChatMessage message) =>
        ChatProvider.SendMessageToSpace(space, message);
}