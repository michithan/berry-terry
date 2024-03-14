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

    public static bool IsPullRequestCommentNotMentioningBot(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("resource", "comment", "content")
            .IsBotMentioned(azureDevOpsClientConfiguration) is false;

    public static bool IsPullRequestCommentFromBot(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("resource", "comment", "author", "id")
            .IsBotIdentityId(azureDevOpsClientConfiguration);

    public static bool IsTicketCommentNotMentioningBot(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("detailedMessage", "markdown")
            .IsBotMentioned(azureDevOpsClientConfiguration) is false;

    public static bool IsTicketCommentFromBot(this JsonElement notificationBody, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        notificationBody
            .GetPropertyValueOrDefault<string>("message", "markdown")
            .ContainsAny($"commented on by {azureDevOpsClientConfiguration.IdentityDisplayName}", $"commented on by {azureDevOpsClientConfiguration.IdentityId}");

    public static bool IsAuthorized(this AuthenticationHeaderValue authenticationHeaderValue, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        authenticationHeaderValue.Scheme == "Basic"
        && authenticationHeaderValue.Parameter?.ConvertFromBase64() == $"{azureDevOpsClientConfiguration.WebhookSecret}:{azureDevOpsClientConfiguration.WebhookSecret}";
}