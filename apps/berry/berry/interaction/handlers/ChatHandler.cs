using berry.interaction.actions;
using berry.interaction.ai;
using leash.chat;

namespace berry.interaction.handlers;

public class ChatHandler(IAiContext aiContext, IChatActor chatActor) : IChatHandler
{
    private IAiContext AiContext { get; init; } = aiContext;

    private IChatActor ChatActor { get; init; } = chatActor;

    public async Task<string?> HandleChatMessage(IChatSpace space, IChatMessage chatMessage)
    {
        var prompt = @$"
        You got a chat message with, the following content:

        {chatMessage.Text}

        Please respond with a kind answerer message.
        ";

        var result = await AiContext.InvokePromptAsync(prompt);

        var answer = result.ToString();
        var responseMessage = chatMessage.CreateAnswer(answer);

        return await ChatActor.AnswerChatMessage(space, responseMessage);
    }
}