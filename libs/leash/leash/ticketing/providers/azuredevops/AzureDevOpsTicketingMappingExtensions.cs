using HtmlAgilityPack;
using leash.clients.azuredevops;
using leash.conversations;
using leash.conversations.provider.azuredevops;
using leash.scm.provider.azuredevops;
using leash.ticketing.ticket.providers.azuredevops;
using leash.utils;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace leash.ticketing.providers.azuredevops;

public static class AzureDevOpsTicketingMappingExtensions
{
    public static AzureDevOpsTicket ToAzureDevOpsTicket(this WorkItem adoWorkItem, CommentList comments, IAzureDevOpsClient azureDevOpsClient) => new()
    {
        Title = adoWorkItem.Fields["System.Title"]?.ToString() ?? string.Empty,
        Description = adoWorkItem.Fields["System.Description"]?.ToString()?.SanitizeAzureDevOpsWorkItemHtml().FromHtmlToMarkdown() ?? string.Empty,
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
        Content = adoComment
            .MapAllIdentityIdMentionsToDisplayNameMentions(azureDevOpsClient)
            .SanitizeAzureDevOpsWorkItemHtml()
            .FromHtmlToMarkdown()
    };

    public static string SanitizeAzureDevOpsWorkItemHtml(this string html) =>
        html
            .ToHtmlDocument()
            ?.SanitizeAnchorNodes()
            .SanitizeUnderlineNodes()
            .DocumentNode
            .OuterHtml ?? html;

    private static HtmlDocument SanitizeAnchorNodes(this HtmlDocument doc)
    {
        var anchorNodes = doc.DocumentNode.SelectNodes("//a");
        if (anchorNodes != null)
        {
            foreach (var anchorNode in anchorNodes)
            {
                anchorNode.Attributes.Remove("data-vss-mention");
            }
        }
        return doc;
    }

    private static HtmlDocument SanitizeUnderlineNodes(this HtmlDocument doc)
    {
        var underlineNodes = doc.DocumentNode.SelectNodes("//u");
        if (underlineNodes != null)
        {
            foreach (var underlineNode in underlineNodes)
            {
                var emNode = HtmlNode.CreateNode("<b>" + underlineNode.InnerHtml + "</b>");
                underlineNode.ParentNode.ReplaceChild(emNode, underlineNode);
            }
        }
        return doc;
    }
}