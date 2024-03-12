using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace berry.interaction.ai;

public sealed class AnthropicClientCore(HttpClient httpClient)
{
    private readonly HttpClient httpClient = httpClient;

    private readonly string Model = "claude-3-opus-20240229";

    private readonly string AnthropicApiUrl = "https://api.anthropic.com/v1/messages";

    public Dictionary<string, object?> Attributes { get; } = [];

    private async Task<List<AnthropicHttpResponseContentBody>> PromptAsync(List<AnthropicHttpRequestMessageBody> messages)
    {
        var requestBody = new AnthropicHttpRequestBody
        {
            model = Model,
            max_tokens = 1024,
            messages = messages,
        };

        var httpResponse = await httpClient.PostAsJsonAsync(AnthropicApiUrl, requestBody);
        var httpResponseBody = await httpResponse.Content.ReadAsStringAsync() ?? throw new Exception("Failed to parse response from Anthropic API");
        var response = JsonSerializer.Deserialize<AnthropicHttpResponseBody>(httpResponseBody) ?? throw new Exception("Failed to parse response from Anthropic API");
        return response.content;
    }

    public async Task<IReadOnlyList<ChatMessageContent>> GetChatMessageContentsAsync(
        ChatHistory chatHistory,
        PromptExecutionSettings? executionSettings = null,
        Kernel? kernel = null,
        CancellationToken cancellationToken = default)
    {
        var messages = chatHistory
            .Select(chat => new AnthropicHttpRequestMessageBody { content = chat.Content ?? string.Empty, role = chat.Role.ToString(), })
            .ToList();

        var responseData = await PromptAsync(messages);

        var defaultResult = responseData
            .Select(content => new ChatMessageContent(role: AuthorRole.Assistant, content: content.text))
            .ToList();

        return defaultResult;
    }

    public async Task<IReadOnlyList<TextContent>> GetChatAsTextContentsAsync(
      string text,
      PromptExecutionSettings? executionSettings,
      Kernel? kernel,
      CancellationToken cancellationToken = default)
    {
        ChatHistory chat = [];
        chat.AddUserMessage(text);
        var chatMessageContent = await GetChatMessageContentsAsync(chat, executionSettings, kernel, cancellationToken);
        return chatMessageContent
            .Select(chat => new TextContent(chat.Content, chat.ModelId, chat.Content, Encoding.UTF8, chat.Metadata))
            .ToList();
    }

    public void AddAttribute(string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            this.Attributes.Add(key, value);
        }
    }
}
