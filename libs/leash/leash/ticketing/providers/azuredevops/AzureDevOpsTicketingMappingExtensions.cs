using leash.conversations;
using leash.conversations.provider.azuredevops;
using leash.ticketing.ticket.providers.azuredevops;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace leash.ticketing.providers.azuredevops;

public static class AzureDevOpsTicketingMappingExtensions
{
    public static AzureDevOpsTicket ToAzureDevOpsTicket(this WorkItem adoWorkItem, CommentList comments, Func<string, bool> isBotMentioned) => new()
    {
        Title = adoWorkItem.Fields["System.Title"]?.ToString() ?? string.Empty,
        Description = adoWorkItem.Fields["System.Description"]?.ToString() ?? string.Empty,
        TicketId = adoWorkItem.Id.ToString() ?? string.Empty,
        TicketUrl = adoWorkItem.Url,
        CommentThread = comments.ToAzureDevOpsThread(isBotMentioned)
    };

    public static AzureDevOpsThread ToAzureDevOpsThread(this CommentList comments, Func<string, bool> isBotMentioned) => new()
    {
        Id = string.Empty,
        Comments = comments.Comments.Select(comment => comment.ToAzureDevOpsComment(isBotMentioned)).ToList<IComment>()
    };

    public static AzureDevOpsComment ToAzureDevOpsComment(this Comment adoComment, Func<string, bool> isBotMentioned) => new()
    {
        Id = adoComment.Id.ToString(),
        Content = adoComment.Text,
        IsBotMentioned = isBotMentioned(adoComment.Text)
    };
}