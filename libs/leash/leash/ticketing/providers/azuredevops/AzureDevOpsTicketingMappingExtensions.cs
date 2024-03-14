using leash.clients.azuredevops;
using leash.conversations;
using leash.conversations.provider.azuredevops;
using leash.scm.provider.azuredevops;
using leash.ticketing.ticket.providers.azuredevops;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace leash.ticketing.providers.azuredevops;

public static class AzureDevOpsTicketingMappingExtensions
{
    public static AzureDevOpsTicket ToAzureDevOpsTicket(this WorkItem adoWorkItem, CommentList comments, IAzureDevOpsClient azureDevOpsClient) => new()
    {
        Title = adoWorkItem.Fields["System.Title"]?.ToString() ?? string.Empty,
        Description = adoWorkItem.Fields["System.Description"]?.ToString() ?? string.Empty,
        TicketId = adoWorkItem.Id.ToString() ?? string.Empty,
        TicketUrl = adoWorkItem.Url,
        CommentThread = comments.ToAzureDevOpsThread(azureDevOpsClient)
    };

    public static AzureDevOpsThread ToAzureDevOpsThread(this CommentList comments, IAzureDevOpsClient azureDevOpsClient) => new()
    {
        Id = string.Empty,
        Comments = comments.Comments.Select(comment => comment.ToAzureDevOpsComment(azureDevOpsClient)).ToList<IComment>()
    };

    public static AzureDevOpsComment ToAzureDevOpsComment(this Comment adoComment, IAzureDevOpsClient azureDevOpsClient) => new()
    {
        Id = adoComment.Id.ToString(),
        Content = adoComment.GetContentWithMentionAsDisplayName(azureDevOpsClient),
        IsBotMentioned = azureDevOpsClient.IsMentionedOnComment(adoComment.Text)
    };
}