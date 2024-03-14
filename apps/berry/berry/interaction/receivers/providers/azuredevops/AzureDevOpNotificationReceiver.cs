using System.Net.Http.Headers;
using System.Text.Json;
using berry.interaction.handlers;
using leash.clients.azuredevops;
using leash.scm.provider.azuredevops;
using leash.ticketing.providers.azuredevops;
using leash.ticketing.ticket;
using leash.utils;

namespace berry.interaction.receivers.providers.azuredevops;

public class AzureDevOpNotificationReceiver(AzureDevOpsClientConfiguration azureDevOpsClientConfiguration, IAzureDevOpsScmProvider azureDevOpsScmProvider, IAzureDevOpsTicketingProvider azureDevOpsTicketingProvider, IPullRequestHandler pullRequestHandler, ITicketHandler ticketHandler) : INotificationReceiver
{
    private AzureDevOpsClientConfiguration AzureDevOpsClientConfiguration { get; init; } = azureDevOpsClientConfiguration;

    private IAzureDevOpsScmProvider AzureDevOpsScmProvider { get; init; } = azureDevOpsScmProvider;

    private IAzureDevOpsTicketingProvider AzureDevOpsTicketingProvider { get; init; } = azureDevOpsTicketingProvider;

    private IPullRequestHandler PullRequestHandler { get; init; } = pullRequestHandler;

    private ITicketHandler TicketHandler { get; init; } = ticketHandler;

    public bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue) => authenticationHeaderValue.IsAuthorized(AzureDevOpsClientConfiguration);

    public Task<string?> ReceiveNotification(JsonElement notificationBody)
    {
        string? eventType = notificationBody.GetPropertyValue<string>("eventType");
        return eventType switch
        {
            "workitem.commented" => HandleWorkItemCommentedNotification(notificationBody),
            "ms.vss-code.git-pullrequest-comment-event" => HandlePullRequestCommentNotification(notificationBody),
            _ => throw new NotImplementedException($"Event type {eventType} is not implemented.")
        };
    }

    public async Task<string?> HandleWorkItemCommentedNotification(JsonElement notificationBody)
    {
        if (notificationBody.IsBotNotMentionedOnTicketComment(AzureDevOpsClientConfiguration))
        {
            return null;
        }

        string ticketId = notificationBody.GetPropertyValueOrDefault<string>("resource", "id");

        ITicket ticket = await AzureDevOpsTicketingProvider.GetTicketByIdAsync(ticketId)
            ?? throw new Exception($"Ticket with id {ticketId} not found.");

        var comment = ticket.CommentThread.Comments.Last();

        return await TicketHandler.HandleTicketComment(ticket, ticket.CommentThread, comment);
    }

    public async Task<string?> HandlePullRequestCommentNotification(JsonElement notificationBody)
    {
        if (notificationBody.IsPullRequestCommentDeleted() || notificationBody.IsPullRequestCommentFromBot(AzureDevOpsClientConfiguration) || notificationBody.IsBotNotMentionedOnPullRequestComment(AzureDevOpsClientConfiguration))
        {
            return null;
        }

        int pullRequestId = notificationBody.GetPropertyValueOrDefault<string>("resource", "pullRequest", "pullRequestId").ToInt();
        int threadId = notificationBody.GetPropertyValueOrDefault<string>("resource", "comment", "_links", "threads", "href").Split('/').Last().ToInt();
        int commentId = notificationBody.GetPropertyValueOrDefault<string>("resource", "comment", "id").ToInt();

        var comment = await AzureDevOpsScmProvider.GetCommentAsync(pullRequestId, threadId, commentId);
        var thread = await AzureDevOpsScmProvider.GetPullRequestThreadByIdAsync(pullRequestId, threadId);
        var pullRequest = await AzureDevOpsScmProvider.GetPullRequestByIdAsync(pullRequestId);

        await PullRequestHandler.HandlePullRequestComment(pullRequest, thread, comment);

        return null;
    }
}