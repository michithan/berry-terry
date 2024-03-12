using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

namespace berry.interaction.ai;

public static class AnthropicConnector
{
    private static readonly string AnthropicVersion = "2023-06-01";

    public static IKernelBuilder AddAnthropicChatCompletion(this IKernelBuilder builder, string apiKey, string? serviceId = null, HttpClient? httpClient = null)
    {
        httpClient ??= new HttpClient();
        httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        httpClient.DefaultRequestHeaders.Add("anthropic-version", AnthropicVersion);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        Func<IServiceProvider, object?, AnthropicChatCompletionService> factory = (serviceProvider, _) =>
        {
            return new AnthropicChatCompletionService(httpClient);
        };

        builder.Services.AddKeyedSingleton<IChatCompletionService>(serviceId, factory);
        builder.Services.AddKeyedSingleton<ITextGenerationService>(serviceId, factory);

        return builder;
    }
}