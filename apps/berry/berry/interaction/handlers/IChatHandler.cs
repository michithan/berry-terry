using leash.chat;

namespace berry.interaction.handlers;

public interface IChatHandler
{
    public Task HandleChatMessage(IChatSpace space, IChatMessage chatMessage);
}