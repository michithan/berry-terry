using System.Net.Http.Headers;
using System.Text.Json;
using leash.clients.azuredevops;
using leash.scm.provider.azuredevops;
using leash.utils;

namespace berry.interaction.receivers.providers.azuredevops;

public static class AzureDevOpNotificationReceiverExtensions
{
    public static bool IsPullRequestCommentDeleted(this JsonElement notificationBody) =>
        notificationBody
            .GetPropertyValueOrDefault<bool>("resource", "comment", "isDeleted");

    public static bool IsBotNotMentionedOnPullRequestComment(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("resource", "comment", "content")
            .IsBotMentioned(azureDevOpsClientConfiguration) is false;

    public static bool IsPullRequestCommentFromBot(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("resource", "comment", "author", "id")
            .IsBotIdentityId(azureDevOpsClientConfiguration);

    public static bool IsBotNotMentionedOnTicketComment(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("detailedMessage")
            .IsBotMentioned(azureDevOpsClientConfiguration) is false;

    public static bool IsAuthorized(this AuthenticationHeaderValue authenticationHeaderValue, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        authenticationHeaderValue.Scheme == "Basic"
        && authenticationHeaderValue.Parameter?.ConvertFromBase64() == $"{azureDevOpsClientConfiguration.WebhookSecret}:{azureDevOpsClientConfiguration.WebhookSecret}";
}