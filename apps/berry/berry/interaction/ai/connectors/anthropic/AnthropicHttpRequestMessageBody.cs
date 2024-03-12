namespace berry.interaction.ai;
#pragma warning disable IDE1006 // Naming Styles

public class AnthropicHttpRequestMessageBody
{
    public required string role { get; init; }

    public required string content { get; init; }
}
