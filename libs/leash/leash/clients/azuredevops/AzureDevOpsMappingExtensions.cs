using System.Text.RegularExpressions;
using leash.clients.azuredevops;

namespace leash.scm.provider.azuredevops;

public static class AzureDevOpsMappingExtensions
{
    private static readonly Regex MentionRegex = new(@"@\<[({]?[a-fA-F0-9]{8}[-]?([a-fA-F0-9]{4}[-]?){3}[a-fA-F0-9]{12}[})]?\>");

    private static string[] GetAllIdentityMentions(this string text) =>
        MentionRegex
            .Matches(text)
            .Select(match => match.Value).ToArray();

    private static string MapIdentityMentionToIdentityId(this string mention) =>
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

    private static string MapIdentityMentionToDisplayNameMention(this string mention, IAzureDevOpsClient azureDevOpsClient) =>
        mention
            .MapIdentityMentionToIdentityId()
            .MapIdentityIdToDisplayName(azureDevOpsClient)
            .MapDisplayNameToMention();

    private static string ReplaceAllIdentityIdsWithDisplayNames(this string text, IAzureDevOpsClient azureDevOpsClient) =>
        text
            .GetAllIdentityMentions()
            .Aggregate(text, (acc, mention) => text.Replace(mention, mention.MapIdentityMentionToDisplayNameMention(azureDevOpsClient)));

    public static string GetContentWithMentionAsDisplayName(this Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment, IAzureDevOpsClient azureDevOpsClient) =>
        comment
            .Content
            .ReplaceAllIdentityIdsWithDisplayNames(azureDevOpsClient);

    public static string GetContentWithMentionAsDisplayName(this Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment comment, IAzureDevOpsClient azureDevOpsClient) =>
        comment
            .Text
            .ReplaceAllIdentityIdsWithDisplayNames(azureDevOpsClient);
}