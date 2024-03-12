using Google.Apis.HangoutsChat.v1.Data;

namespace leash.clients.google;

public interface IGoogleClient
{
    public void SendMessage(Message message, string spaceName);
}