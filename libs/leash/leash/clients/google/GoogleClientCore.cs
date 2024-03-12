using Google.Apis.Auth.OAuth2;
using Google.Apis.HangoutsChat.v1;
using Google.Apis.Services;

namespace leash.clients.google;

public class GoogleClientCore(GoogleClientConfiguration googleClientConfiguration)
{
    public HangoutsChatService HangoutsChatService => ConnectHangoutsChatService();

    private GoogleClientConfiguration GoogleClientConfiguration { get; init; } = googleClientConfiguration;

    private readonly string[] ClientScopes = ["https://www.googleapis.com/auth/chat.bot"];

    private string GetAccessToken()
    {
        ClientSecrets clientSecrets = new()
        {
            ClientId = GoogleClientConfiguration.ClientId,
            ClientSecret = GoogleClientConfiguration.ClientSecret
        };
        var userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        clientSecrets,
                        ClientScopes,
                        GoogleClientConfiguration.UserName,
                        CancellationToken.None)
                        .Result;
        return userCredential.Token.AccessToken;
    }

    private BaseClientService.Initializer GetBaseClientServiceInitializer()
    {
        string accessToken = GetAccessToken();
        var credentials = GoogleCredential.FromAccessToken(accessToken);
        return new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            GZipEnabled = false,
        };
    }

    private HangoutsChatService ConnectHangoutsChatService()
    {
        var initializer = GetBaseClientServiceInitializer();
        return new HangoutsChatService(initializer);
    }
}