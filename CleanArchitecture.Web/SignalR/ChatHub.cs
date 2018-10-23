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
        private RedisConnectionFactory _fact;
        public Chat(RedisConnectionFactory factory)
        {
            _fact = factory;
        }

        //For Testing Connection
        public void SendToAll(string name, string message)
        {
            Clients.All.SendAsync("sendToAll", name, message);
        }

        // Add for Subscription Channel
        public override Task OnConnectedAsync()
        {
            Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast").Wait();            
            return base.OnConnectedAsync();
        }

        public void OnConnected(string UserID, string AccessToken, string Username)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.SaveToHash(UserID + ":ConnectionDetail", new ConnetedClientList { ConnectionId = Context.ConnectionId }, Context.ConnectionId);
            var Redis1 = new RadisServices<ConnetedClientToken>(this._fact);
            Redis1.SaveToHash(UserID + ":Token", new ConnetedClientToken { Token = AccessToken }, Context.ConnectionId);
            var Redis2 = new RadisServices<ConnetedUserDetail>(this._fact);
            Redis2.SaveToHash(UserID + ":UserDetail", new ConnetedUserDetail { UserName = Username }, Context.ConnectionId);
            //var Redis3 = new RadisServices<BlockUserDetail>(this._fact);
            //BlockUserDetail User = new BlockUserDetail();
            //User = Redis3.GetConnectionID(UserID + ":BlockDetail");
            //if (string.IsNullOrEmpty(User.ToString()) && !User.IsBlock)
            //{
            //    Groups.AddToGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
            //}
            //else
            //{
            //    // on this action Remove text box from Chat for this client
            //    Clients.Client(Context.ConnectionId).SendAsync("BlockedUser", User.ToString());
            //}            
            //Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast");
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.Scan(Context.ConnectionId, ":ConnectionDetail");
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BroadCast").Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
            return base.OnDisconnectedAsync(exception);
        }

        // Remove From Subscription Channel
        public void RemoveSubscription(string ChannelName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, ChannelName).Wait();
        }

        // Add to Subscription Channel
        public void AddSubscription(string ChannelName)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, ChannelName).Wait();
        }

        // one to one Message Chat
        public void SendChatMessage(string UserID, string Message)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            Clients.Client(User.ConnectionId).SendAsync("RecieveMessage", User + ": " + Message);
            //Clients.Group("BroadCast").SendAsync("BroadcastMessage", User + ": " + message);
        }

        public void PendingOrderUpdate(string UserID, string Data)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            Clients.Client(User.ConnectionId).SendAsync("RecievePendingOrderUpdate", Data);
            //Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
        }

        public void SattledOrderUpdate(string Data)
        {
            Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
        }

        public void TransactionHistory(string Data)
        {
            Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
        }

        public void SendGroupMessage(string Name, string Message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", Name, Message);
            var Redis = new RadisServices<ChatHistory>(this._fact);
            Redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = Name, Message = Message, Id = Guid.NewGuid() }, Name);
            Redis.GetSetData("GroupChatHistory");
        }

        public void GetChatHistory(string Name, string Message)
        {
            var Redis = new RadisServices<ChatHistory>(this._fact);
            string Data = Redis.GetSetData("GroupChatHistory");
            Clients.Client(Context.ConnectionId).SendAsync("RecieveChatHistory", Data);
        }

        public void BlockedUser(string UserID)
        {
            // Add to blocklist
            var Redis2 = new RadisServices<BlockUserDetail>(this._fact);
            Redis2.SaveToHash(UserID + ":BlockDetail", new BlockUserDetail { IsBlock = true }, Context.ConnectionId);

            // Call the broadcastMessage method to update clients.
            //Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
            //var redis = new RadisServices<ChatHistory>(this._fact);
            //redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
            //redis.GetSetData("GroupChatHistory");
        }       

    }
}