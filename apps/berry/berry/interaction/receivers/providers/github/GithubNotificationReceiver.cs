using System.Net.Http.Headers;
using System.Text.Json;
using leash.utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;
using Octokit.Webhooks.Events.IssueComment;

namespace berry.interaction.receivers.providers.github;

public class GithubNotificationReceiver(GithubWebhookEventProcessor githubWebhookEventProcessor) : NotificationReceiverBase
{
    GithubWebhookEventProcessor GithubWebhookEventProcessor { get; init; } = githubWebhookEventProcessor;

    public override bool IsAuthorized(AuthenticationHeaderValue authenticationHeaderValue)
    {
        throw new NotImplementedException();
    }

    public override Task ReceiveNotification(JsonElement notificationBody, Action<Func<object, OkObjectResult>>? callback = null)
    {
        var headers = new Dictionary<string, StringValues>
        {
            { "X-GitHub-Event", notificationBody.GetPropertyValueOrDefault<string>("action") }
        };
        var webhookHeaders = WebhookHeaders.Parse(headers);
        var webhookBody = notificationBody.ToCompactJsonString();
        var webhookEvent = GithubWebhookEventProcessor.DeserializeWebhookEvent(webhookHeaders, webhookBody);

        HandleWebhookEvent(webhookEvent);

        return Task.CompletedTask;
    }

    private Task HandleWebhookEvent(WebhookEvent webhookEvent) =>
        webhookEvent switch
        {
            IssueCommentCreatedEvent issueCommentCreatedEvent => HandleIssueCommentCreatedEvent(issueCommentCreatedEvent),
            PullRequestReviewCommentEvent pullRequestReviewCommentEvent => HandlePullRequestReviewCommentEvent(pullRequestReviewCommentEvent),
            _ => Task.CompletedTask
        };

    private Task HandleIssueCommentCreatedEvent(IssueCommentCreatedEvent issueCommentCreatedEvent)
    {
        throw new NotImplementedException();
    }

    private Task HandlePullRequestReviewCommentEvent(PullRequestReviewCommentEvent pullRequestReviewCommentEvent)
    {
        throw new NotImplementedException();
    }
}