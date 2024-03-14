namespace leash.scm.provider.azuredevops;

public static class AzureDevOpsMappingExtensions
{
    private static string ReplaceIdentityIdWithDisplayName(string text, string identityId, string displayName) =>
        text.Replace($"@<{identityId}>", $"@{displayName}");

    public static string GetContentWithMentionAsDisplayName(this Microsoft.TeamFoundation.SourceControl.WebApi.Comment comment) =>
        ReplaceIdentityIdWithDisplayName(comment.Content, comment.Author.Id, comment.Author.DisplayName);

    public static string GetContentWithMentionAsDisplayName(this Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.Comment comment) =>
        ReplaceIdentityIdWithDisplayName(comment.Text, comment.CreatedBy.Id, comment.CreatedBy.DisplayName);
}