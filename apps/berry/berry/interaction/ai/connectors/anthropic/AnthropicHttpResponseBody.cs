namespace berry.interaction.ai;
#pragma warning disable IDE1006 // Naming Styles

public class AnthropicHttpResponseBody
{
    public required string id { get; init; }

    public required string type { get; init; }

    public required string role { get; init; }

    public required List<AnthropicHttpResponseContentBody> content { get; init; }

    public required string model { get; init; }

    public required string stop_reason { get; init; }

    public required string? stop_sequence { get; init; }

    public required AnthropicHttpResponseUsageBody usage { get; init; }
}
