namespace API.SignalR
{
    //This class is not scalable.
    //Change it later to store the onlineUsers inside some database
    public class StatusTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = 
            new Dictionary<string,  List<string>>();  

        public bool UserConnected(string username, string connectionId)
        {
            bool newOnline = false;

            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>() { connectionId });
                    newOnline = true;
                }
            }

            return newOnline;
        }

        public bool UserDisconnected(string username, string connectionid)
        {
            bool newOffline = false;

            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username)) return newOffline;

                OnlineUsers[username].Remove(connectionid);

                if (OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    newOffline = true;
                }
            }

            return newOffline;
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

        public static List<string> GetUserConnections(string username)
        {
            List<string> connectionIds;

            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return connectionIds;
        }
    }
}