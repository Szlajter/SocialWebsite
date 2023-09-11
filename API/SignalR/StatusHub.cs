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
            var newOnline = _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            if (newOnline)
            {
                await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            }

            var currentUsers = _tracker.GetOnlineUsers();
            await Clients.Caller.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var newOffline = _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);

            if (newOffline)
            {
                await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            }           
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}