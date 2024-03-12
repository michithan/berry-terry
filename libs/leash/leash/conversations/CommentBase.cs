namespace leash.conversations;

public abstract class CommentBase : IComment
{
    public required string Id { get; init; }

    public bool IsBotMentioned { get; init; }

    public required string Content { get; init; }

    public IComment CreateAnswer(string content)
    {
        Type type = GetType() ?? throw new InvalidOperationException("Could not get type");
        IComment answer = Activator.CreateInstance(GetType()) as IComment ?? throw new InvalidOperationException("Could not create answer");
        type.GetProperty("Content")?.SetValue(answer, content);
        type.GetProperty("IsBotMentioned")?.SetValue(answer, IsBotMentioned);
        return answer;
    }
}