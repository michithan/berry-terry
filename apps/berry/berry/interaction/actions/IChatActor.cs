using leash.chat;

namespace berry.interaction.actions;

public interface IChatActor
{
    public Task<string?> AnswerChatMessage(IChatSpace space, IChatMessage message);
}