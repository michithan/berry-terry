using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace berry.tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly JsonElement AdoWorkItemCommentMock = WebHookTestExtensions.LoadJsonMockFile("AzureDevOps.ticket_comment.json");

    private readonly WebApplicationFactory<Program> _factory;


    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/api/webhooks/ado")]
    public async Task Get_EndpointsReturnInternalServerErrorContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false
        };
        var jsonBody = JsonSerializer.Serialize(AdoWorkItemCommentMock, options);

        // Act
        HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode); // Status Code 500
    }
}