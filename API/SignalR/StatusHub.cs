using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    //Responsible for user's status: online, offline
    [Authorize]
    public class StatusHub: Hub
    {
        private readonly StatusTracker _tracker;

        public StatusHub(StatusTracker tracker)
        {
            _tracker = tracker;
            
        }

        public override async Task OnConnectedAsync()
        {
            _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var currentUsers = _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

            var currentUsers = _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
    }
}