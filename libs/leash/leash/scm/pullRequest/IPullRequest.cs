using leash.conversations;

namespace leash.scm.pullRequest;

public interface IPullRequest
{
    public string Id { get; }

    public string Title { get; }

    public string Description { get; }

    public string SourceBranch { get; }

    public string TargetBranch { get; }

    public string Url { get; }

    public IEnumerable<IThread> Threads { get; }
}