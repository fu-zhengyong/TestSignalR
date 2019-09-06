using System;
using System.Collections.Concurrent;
using System.Web;
using Microsoft.AspNetCore.SignalR;

namespace TestSignalR.Hubs
{
    public class ChatHub : Hub
    {
        public static ConcurrentDictionary<string, string> OnLineUsers = new ConcurrentDictionary<string, string>();

        public void Send(string message)
        {
            string clientName = OnLineUsers[Context.ConnectionId];
            message = HttpUtility.HtmlEncode(message).Replace("\r\n", "<br/>").Replace("\n", "<br/>");
            Clients.All.SendAsync("receiveMessage", DateTime.Now.ToString("yyyy -MM-dd HH:mm:ss"), clientName, message);
        }

        public void SendOne(string toUserId, string message)
        {
            string clientName = OnLineUsers[Context.ConnectionId];
            message = HttpUtility.HtmlEncode(message).Replace("\r\n", "<br/>").Replace("\n", "<br/>");
            Clients.Caller.SendAsync("receiveMessage",DateTime.Now.ToString("yyyy -MM-dd HH:mm:ss"), string.Format("您对 {1}", clientName, OnLineUsers[toUserId]), message);
            Clients.Client(toUserId).SendAsync("receiveMessage",DateTime.Now.ToString("yyyy -MM-dd HH:mm:ss"), string.Format("{0} 对您", clientName), message);
        }

        public override System.Threading.Tasks.Task OnConnectedAsync()
        {
            string clientName = Context.ConnectionId;
            OnLineUsers.AddOrUpdate(Context.ConnectionId, clientName, (key, value) => clientName);
            Clients.All.SendAsync("userChange", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("{0} 加入了。", clientName), OnLineUsers.ToArray());
            return base.OnConnectedAsync();
        }

        public override System.Threading.Tasks.Task OnDisconnectedAsync(Exception exception)
        {
            string clientName = Context.ConnectionId;
            Clients.All.SendAsync("userChange", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), string.Format("{0} 离开了。", clientName), OnLineUsers.ToArray());
            OnLineUsers.TryRemove(Context.ConnectionId, out clientName);
            return base.OnDisconnectedAsync(exception);
        }

    }
}
