using leash.chat;

namespace berry.interaction.handlers;

public interface IChatHandler
{
    public Task<string?> HandleChatMessage(IChatSpace space, IChatMessage chatMessage);
}