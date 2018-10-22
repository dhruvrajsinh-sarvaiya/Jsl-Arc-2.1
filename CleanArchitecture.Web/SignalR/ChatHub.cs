using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.ApiModels.Chat;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Web.SignalR
{
    public class Chat : Hub
    {
        //private static IOptions<RedisConfiguration> redis;
        private RedisConnectionFactory _fact;
        //= new RedisConnectionFactory(redis);
        public Chat(RedisConnectionFactory factory)
        {
            _fact = factory;
        }
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", message);
        }
        public void SendChatMessage(long UserID, string message)
        {
            // string name = Context.User.Identity.Name;
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + "ConnectionID");
            //foreach (var connectionId in _connections.GetConnections(who))
            //{
            Clients.Client(User.ConnectionId).SendAsync("RecieveMessage", User + ": " + message);
            Clients.Group("BroadCast").SendAsync("BroadcastMessage", User + ": " + message);
            // Clients.All.SendAsync("BroadcastMessage", token + ": " + message);
            //}
        }

        public void SendGroupMessage(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
            var Redis = new RadisServices<ChatHistory>(this._fact);
            Redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
            Redis.GetSetData("GroupChatHistory");
        }

        public void BlockedUsers(string id)
        {
            // Call the broadcastMessage method to update clients.
            //Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
            //var redis = new RadisServices<ChatHistory>(this._fact);
            //redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
            //redis.GetSetData("GroupChatHistory");
        }

        public override Task OnConnectedAsync()
        {
            //string name = Context.User.Identity.Name;

            //_connections.Add(name, Context.ConnectionId);
            Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast").Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
            return base.OnConnectedAsync();
        }

        public void OnConnected(long  UserID,string AccessToken,string Username)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            //_connections.Add(token, Context.ConnectionId);
            Redis.SaveToHash(UserID + "ConnectionID", new ConnetedClientList { ConnectionId = Context.ConnectionId }, Context.ConnectionId);
            var Redis1 = new RadisServices<ConnetedClientToken>(this._fact);
            Redis1.SaveToHash(UserID + "Token", new ConnetedClientToken { Token  = AccessToken }, Context.ConnectionId);
            var Redis2 = new RadisServices<ConnetedUserDetail>(this._fact);
            Redis2.SaveToHash(UserID + "Username", new ConnetedUserDetail { UserName = Username }, Context.ConnectionId);

            //Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast");
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //string name = Context.User.Identity.Name;
            var redis = new RadisServices<ConnetedClientList>(this._fact);
            redis.scan(Context.ConnectionId);
            //_connections.Remove(name, Context.ConnectionId);
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BroadCast").Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
            return base.OnDisconnectedAsync(exception);
        }

    }
}