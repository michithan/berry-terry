using leash.chat;

namespace berry.interaction.actions;

public interface IChatActor
{
    public Task AnswerChatMessage(IChatSpace space, IChatMessage message);
}