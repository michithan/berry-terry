using Microsoft.SemanticKernel;

namespace berry.interaction.ai;

public interface IAiContext
{
    Task<FunctionResult> InvokePromptAsync(string prompt);
}