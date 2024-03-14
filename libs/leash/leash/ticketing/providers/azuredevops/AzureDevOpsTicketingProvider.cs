using leash.clients.azuredevops;
using leash.conversations;
using leash.ticketing.ticket;
using leash.utils;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace leash.ticketing.providers.azuredevops;

public class AzureDevOpsTicketingProvider(IAzureDevOpsClient azureDevOpsClient) : TicketingProviderBase, IAzureDevOpsTicketingProvider
{
    IAzureDevOpsClient AzureDevOpsClient { get; init; } = azureDevOpsClient;

    public override async Task<ITicket?> GetTicketByIdAsync(string id)
    {
        int workItemId = id.ToInt();
        var adoWorkItem = await AzureDevOpsClient.GetWorkItemByIdAsync(workItemId) ?? throw new Exception($"Ticket with id {id} not found");
        var comments = await AzureDevOpsClient.GetWorkItemCommentsAsync(workItemId);
        return adoWorkItem.ToAzureDevOpsTicket(comments, AzureDevOpsClient);
    }

    public override async Task<IComment?> GetTicketCommentByIdAsync(string ticketId, string commentId)
    {
        var ticket = await GetTicketByIdAsync(ticketId);
        return ticket?.CommentThread.Comments.FirstOrDefault(comment => comment.Id == commentId);
    }

    public override async Task CommentOnTicketAsync(ITicket ticket, IThread _, IComment comment)
    {
        CommentCreate commentCreate = new()
        {
            Text = comment.Content
        };
        await AzureDevOpsClient.CreateWorkItemCommentAsync(commentCreate, ticket.TicketId.ToInt());
    }
}