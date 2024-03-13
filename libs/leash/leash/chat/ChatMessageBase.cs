namespace leash.chat;

public abstract class ChatMessageBase : IChatMessage
{
    public required string Text { get; init; }

    public required bool IsBotMentioned { get; init; }

    public IChatMessage CreateAnswer(string content)
    {
        Type type = GetType() ?? throw new InvalidOperationException("Could not get type");
        IChatMessage answer = Activator.CreateInstance(GetType()) as IChatMessage ?? throw new InvalidOperationException("Could not create answer");
        type.GetProperty("Text")?.SetValue(answer, content);
        type.GetProperty("IsBotMentioned")?.SetValue(answer, IsBotMentioned);
        return answer;
    }
}