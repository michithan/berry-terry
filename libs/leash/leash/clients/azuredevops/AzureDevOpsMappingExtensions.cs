using System.Text.RegularExpressions;
using leash.clients.azuredevops;
using leash.utils;

namespace leash.scm.provider.azuredevops;

public static class AzureDevOpsMappingExtensions
{
    private static readonly Regex MentionRegex = new(@"@\<[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?\>");

    private static string[] GetAllIdentityIdMentions(this string text) =>
        MentionRegex
            .Matches(text)
            .Select(match => match.Value).ToArray();

    private static string MapIdentityIdMentionToIdentityId(this string mention) =>
        mention
            .Replace("@<", string.Empty)
            .Replace(">", string.Empty);

    private static string MapIdentityIdToDisplayName(this string identityId, IAzureDevOpsClient azureDevOpsClient) =>
        azureDevOpsClient
            .GetIdentityById(identityId)
            .Result
            .DisplayName;

    private static string MapDisplayNameToMention(this string displayName) =>
        $"@<{displayName}>";

    private static string MapIdentityIdMentionToDisplayNameMention(this string mention, IAzureDevOpsClient azureDevOpsClient) =>
        mention
            .MapIdentityIdMentionToIdentityId()
            .MapIdentityIdToDisplayName(azureDevOpsClient)
            .MapDisplayNameToMention();

    private static string MapAllIdentityIdMentionsToDisplayNameMentions(this string text, IAzureDevOpsClient azureDevOpsClient) =>
        text
            .GetAllIdentityIdMentions()
            .Aggregate(text, (acc, mention) => text.Replace(mention, mention.MapIdentityIdMentionToDisplayNameMention(azureDevOpsClient)));

    public static string MapAllIdentityIdMentionsToDisplayNameMentions(this Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment, IAzureDevOpsClient azureDevOpsClient) =>
        comment
            .Content
            .MapAllIdentityIdMentionsToDisplayNameMentions(azureDevOpsClient);

    public static string MapAllIdentityIdMentionsToDisplayNameMentions(this Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment comment, IAzureDevOpsClient azureDevOpsClient) =>
        comment
            .Text
            .MapAllIdentityIdMentionsToDisplayNameMentions(azureDevOpsClient);

    public static bool IsBotMentioned(this string text, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        text
            .ContainsAny(azureDevOpsClientConfiguration.IdentityId, azureDevOpsClientConfiguration.IdentityDisplayName);

    public static bool IsBotIdentityId(this string identityId, AzureDevOpsClientConfiguration azureDevOpsClientConfiguration) =>
        identityId
            .Equals(azureDevOpsClientConfiguration.IdentityId, StringComparison.OrdinalIgnoreCase);
}