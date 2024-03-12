using leash.clients.azuredevops;
using leash.conversations.provider.azuredevops;
using leash.scm.provider;
using leash.scm.provider.azuredevops;
using leash.scm.pullRequest;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using NSubstitute;

namespace leash.tests;

[TestClass]
public class AzureDevOpsClientTest
{
    private string Organization { get; init; }

    private string Project { get; init; }

    private string Repository { get; init; }

    private string Token { get; init; }

    private AzureDevOpsClientConfiguration Configuration { get; init; }

    private IAzureDevOpsClient MockAzureDevOpsConnection { get; init; }

    private GitPullRequest PullRequest { get; init; }

    public AzureDevOpsClientTest()
    {
        // Arrange
        Token = "token";
        Organization = "organization";
        Project = "project";
        Repository = "repository";
        Configuration = new AzureDevOpsClientConfiguration()
        {
            Organization = Organization,
            Project = Project,
            RepositoryId = Repository,
            Token = Token,
            WebhookSecret = Guid.NewGuid().ToString()
        };

        PullRequest = new()
        {
            Title = "title",
            Description = "description",
            SourceRefName = "source",
            TargetRefName = "target"
        };

        MockAzureDevOpsConnection = Substitute.For<IAzureDevOpsClient>();

        MockAzureDevOpsConnection.GetAllPullRequestsAsync().Returns([PullRequest]);
        MockAzureDevOpsConnection.GetPullRequestByIdAsync(Arg.Any<int>()).Returns(PullRequest);
        MockAzureDevOpsConnection.CreatePullRequestCommentAsync(Arg.Any<Comment>(), Arg.Any<int>(), Arg.Any<int>()).Returns(Task.CompletedTask);
    }

    [TestMethod]
    public void AzureDevOpsClient_Should_Create()
    {
        // Act
        var provider = new AzureDevOpsScmProvider(MockAzureDevOpsConnection);

        // Assert
        Assert.IsNotNull(provider);
        Assert.IsInstanceOfType(provider, typeof(IScmProvider));
    }

    [TestMethod]
    public async Task AzureDevOpsClient_Should_GetAllPullRequestsAsync()
    {
        // Arrange
        var provider = new AzureDevOpsScmProvider(MockAzureDevOpsConnection);

        // Act
        var pullRequests = await provider.GetAllPullRequestsAsync();

        // Assert
        Assert.IsNotNull(pullRequests);
        Assert.IsInstanceOfType(pullRequests, typeof(IEnumerable<IPullRequest>));
        Assert.AreEqual(1, pullRequests.Count());
        Assert.AreEqual(PullRequest.PullRequestId.ToString(), pullRequests.First().Id);
    }

    [TestMethod]
    public async Task AzureDevOpsClient_Should_GetPullRequestByIdAsync()
    {
        // Arrange
        var provider = new AzureDevOpsScmProvider(MockAzureDevOpsConnection);

        // Act
        var pullRequest = await provider.GetPullRequestByIdAsync(1);

        // Assert
        Assert.IsNotNull(pullRequest);
        Assert.IsInstanceOfType(pullRequest, typeof(IPullRequest));
        Assert.AreEqual(PullRequest.PullRequestId.ToString(), pullRequest.Id);
        Assert.AreEqual(PullRequest.Title, pullRequest.Title);
    }

    [TestMethod]
    public async Task AzureDevOpsClient_CommentOnPullRequestThreadAsync()
    {
        // Arrange
        var provider = new AzureDevOpsScmProvider(MockAzureDevOpsConnection);
        var pullRequest = new AzureDevOpsPullRequest()
        {
            Id = "1",
            Title = "title",
            Description = "description",
            SourceBranch = "source",
            TargetBranch = "target",
            Url = string.Empty,
            Threads = []
        };
        var thread = new AzureDevOpsThread()
        {
            Id = "1",
            Comments = []
        };
        var comment = new AzureDevOpsComment()
        {
            Id = "1",
            IsBotMentioned = true,
            Content = "content"
        };

        // Act
        await provider.CommentOnPullRequestThreadAsync(pullRequest, thread, comment);
    }
}