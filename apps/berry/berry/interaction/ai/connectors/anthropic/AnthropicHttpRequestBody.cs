namespace berry.interaction.ai;
#pragma warning disable IDE1006 // Naming Styles

public class AnthropicHttpRequestBody
{
    public required string model { get; init; }

    public required int max_tokens { get; init; }

    public required List<AnthropicHttpRequestMessageBody> messages { get; init; }
}
