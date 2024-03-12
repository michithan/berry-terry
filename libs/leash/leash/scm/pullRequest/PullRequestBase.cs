using leash.conversations;

namespace leash.scm.pullRequest;

public abstract class PullRequestBase : IPullRequest
{
    public required string Id { get; init; }

    public required string Title { get; init; }

    public required string Description { get; init; }

    public required string SourceBranch { get; init; }

    public required string TargetBranch { get; init; }

    public required string Url { get; init; }

    public required IEnumerable<IThread> Threads { get; init; }
}