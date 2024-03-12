using System.Configuration;
using berry.configuration;
using Microsoft.SemanticKernel;

namespace berry.interaction.ai;

public class AiContext : IAiContext
{
    private Kernel Kernel { get; }

    public AiContext(AiConfiguration AiConfiguration)
    {
        var builder = Kernel.CreateBuilder();

        if (!string.IsNullOrEmpty(AiConfiguration.Model) && !string.IsNullOrEmpty(AiConfiguration.AzureEndpoint) && !string.IsNullOrEmpty(AiConfiguration.ApiKey))
        {
            builder.AddAzureOpenAIChatCompletion(AiConfiguration.Model, AiConfiguration.AzureEndpoint, AiConfiguration.ApiKey);
            builder.AddAzureOpenAITextGeneration(AiConfiguration.Model, AiConfiguration.AzureEndpoint, AiConfiguration.ApiKey);
        }
        else if (!string.IsNullOrEmpty(AiConfiguration.AnthropicApiKey))
        {
            builder.AddAnthropicChatCompletion(AiConfiguration.AnthropicApiKey);
        }
        else
        {
            throw new ConfigurationErrorsException("AiConfiguration is required. Please check the configuration file.");
        }

        Kernel = builder.Build();
    }

    public Task<FunctionResult> InvokePromptAsync(string prompt) => Kernel.InvokePromptAsync(prompt);
}