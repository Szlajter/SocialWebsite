namespace API.SignalR
{
    //This class is not scalable.
    //Change it later to store the onlineUsers inside some database
    public class StatusTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = 
            new Dictionary<string,  List<string>>();  

        public void UserConnected(string username, string connectionId)
        {
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>() { connectionId });
                }
            }
        }

        public void UserDisconnected(string username, string connectionid)
        {
            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username)) return;

                OnlineUsers[username].Remove(connectionid);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                }
            }
        }

        public string[] GetOnlineUsers()
        {
            string[] onlineUsers;

            lock(OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return onlineUsers;
        }
    }
}