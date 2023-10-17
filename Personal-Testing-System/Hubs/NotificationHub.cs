using Microsoft.AspNetCore.SignalR;

namespace Personal_Testing_System.Hubs
{
    public sealed class NotificationHub : Hub<INotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            //await Clients.All.ReceiveMessage($"connectionId:{Context.ConnectionId}");
        }

        public async Task SendMessageAll(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}
