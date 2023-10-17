namespace Personal_Testing_System.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveMessage(string message);
        Task SendMessage(string message);
    }
}
