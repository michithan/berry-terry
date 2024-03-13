using Google.Apis.HangoutsChat.v1.Data;

namespace leash.clients.google;

public interface IGoogleClient
{
    public void SendMessageToSpace(Message message, string spaceName);

    public List<Message> GetAllUnreadMessages();
}