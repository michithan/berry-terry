using Google.Apis.HangoutsChat.v1.Data;

namespace leash.chat.providers.google;

public static class GoogleChatMappingExtensions
{
    public static Message ToGoogleChatMessage(this IChatMessage message) =>
         new()
         {
             Text = message.Text
         };

    public static Space ToGoogleChatSpace(this IChatSpace space) =>
         new()
         {
             Name = space.Name
         };

    public static GoogleChatMessage ToChatMessage(this Message message) =>
        new()
        {
            Text = message.Text
        };

    public static GoogleChatSpace ToChatSpace(this Space space) =>
        new()
        {
            Name = space.Name
        };
}