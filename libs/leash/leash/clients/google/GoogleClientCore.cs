using Google.Apis.Auth.OAuth2;
using Google.Apis.HangoutsChat.v1;
using Google.Apis.Services;
using static Google.Apis.Auth.OAuth2.ServiceAccountCredential;

namespace leash.clients.google;

public class GoogleClientCore(GoogleClientConfiguration googleClientConfiguration)
{
    public HangoutsChatService HangoutsChatService => ConnectHangoutsChatService();

    private GoogleClientConfiguration GoogleClientConfiguration { get; init; } = googleClientConfiguration;

    private readonly string[] ClientScopes = ["https://www.googleapis.com/auth/chat.bot"];

    private ServiceAccountCredential GetServiceAccountCredentials() =>
        GoogleCredential
            .FromJson(GoogleClientConfiguration.AccessKeyJson)
            .CreateScoped(ClientScopes)
            .UnderlyingCredential as ServiceAccountCredential ?? throw new Exception("Failed to create ServiceAccountCredential");

    private Initializer GetInitializer(ServiceAccountCredential serviceAccountCredentials) =>
        new(serviceAccountCredentials.Id)
        {
            User = GoogleClientConfiguration.UserName,
            Key = serviceAccountCredentials.Key,
            Scopes = ClientScopes
        };

    private Initializer GetServiceAccountInitializer()
    {
        var serviceAccountCredentials = GetServiceAccountCredentials();
        return GetInitializer(serviceAccountCredentials);
    }

    private HangoutsChatService ConnectHangoutsChatService()
    {
        var initializer = GetServiceAccountInitializer();
        var userCredentials = new ServiceAccountCredential(initializer);
        userCredentials.RequestAccessTokenAsync(CancellationToken.None).Wait();

        var initializerHangouts = new BaseClientService.Initializer
        {
            HttpClientInitializer = userCredentials,
            ApplicationName = GoogleClientConfiguration.ApplicationName
        };

        return new HangoutsChatService(initializerHangouts);
    }
}