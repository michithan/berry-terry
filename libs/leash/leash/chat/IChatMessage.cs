namespace leash.chat;

public interface IChatMessage
{
    public string Text { get; init; }

    public bool IsBotMentioned { get; init; }

    public IChatMessage CreateAnswer(string content);
}