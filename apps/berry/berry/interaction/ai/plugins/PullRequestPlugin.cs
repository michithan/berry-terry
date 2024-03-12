using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace berry.interaction.ai.plugins;

public class PullRequestPlugin
{
    [KernelFunction]
    [Description("Respond after being mentioned on a pullrequest comment.")]
    public async Task RespondAfterMentionOnPullRequestComment(
        Kernel kernel,
        [Description("Semicolon delimitated list of emails of the recipients")] string recipientEmails,
        string subject,
        string body
    )
    {
        // Add logic to send an email using the recipientEmails, subject, and body
        // For now, we'll just print out a success message to the console
        Console.WriteLine("Email sent!");
        await Task.CompletedTask;
    }
}