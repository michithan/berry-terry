using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.TextGeneration;

namespace berry.interaction.ai;

public sealed class AnthropicChatCompletionService(HttpClient httpClient) : IChatCompletionService, ITextGenerationService
{
    private readonly AnthropicClientCore _core = new(httpClient);

    public IReadOnlyDictionary<string, object?> Attributes => this._core.Attributes;

    public Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) =>
        this._core.GetChatMessageContentsAsync(chatHistory, executionSettings, kernel, cancellationToken);

    public IAsyncEnumerable<StreamingChatMessageContent> GetStreamingChatMessageContentsAsync(ChatHistory chatHistory, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<IReadOnlyList<TextContent>> GetTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) =>
        this._core.GetChatAsTextContentsAsync(prompt, executionSettings, kernel, cancellationToken);

    public IAsyncEnumerable<StreamingTextContent> GetStreamingTextContentsAsync(string prompt, PromptExecutionSettings? executionSettings = null, Kernel? kernel = null, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();
}
