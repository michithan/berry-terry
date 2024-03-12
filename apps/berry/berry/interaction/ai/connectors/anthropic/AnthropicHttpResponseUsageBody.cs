namespace berry.interaction.ai;
#pragma warning disable IDE1006 // Naming Styles

public class AnthropicHttpResponseUsageBody
{
    public required int input_tokens { get; init; }

    public required int output_tokens { get; init; }
}
