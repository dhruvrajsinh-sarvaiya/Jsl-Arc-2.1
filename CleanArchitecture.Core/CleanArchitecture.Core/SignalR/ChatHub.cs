using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.ApiModels.Chat;
using Microsoft.Extensions.Options;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Wallet;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using CleanArchitecture.Core.Enums;
using System.Linq;

namespace CleanArchitecture.Core.SignalR
{
    public class SocketHub : Hub
    {
        private RedisConnectionFactory _fact;
        private IHubContext<SocketHub> _chatHubContext;
        public SocketHub(IHubContext<SocketHub> ChatHubContext,RedisConnectionFactory Factory)
        {
            _fact = Factory;
            _chatHubContext = ChatHubContext;
        }

        //For Testing Connection
        public Task SendToAll(string name, string message)
        {
            _chatHubContext.Clients.All.SendAsync("sendToAll", name, message);
            return Task.FromResult(0);
        }

        //For Testing only
        public Task SattledOrderUpdate(string Data)
        {
            _chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
            return Task.FromResult(0);
        }

        // Add for Subscription Channel
        public override Task OnConnectedAsync()
        {
            string Pair = "LTC_BTC";
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.SaveTagsToSetMember("Pairs:" + Pair, Context.ConnectionId, Pair); 
            Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast").Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
            return base.OnConnectedAsync();
        }

        //public void OnConnected(string UserID, string AccessToken, string Username)
        //{
        //    var Redis = new RadisServices<ConnetedClientList>(this._fact);
        //    Redis.SaveToHash(UserID + ":ConnectionDetail", new ConnetedClientList { ConnectionId = Context.ConnectionId }, Context.ConnectionId);
        //    var Redis1 = new RadisServices<ConnetedClientToken>(this._fact);
        //    Redis1.SaveToHash(UserID + ":Token", new ConnetedClientToken { Token = AccessToken }, Context.ConnectionId);
        //    var Redis2 = new RadisServices<ConnetedUserDetail>(this._fact);
        //    Redis2.SaveToHash(UserID + ":UserDetail", new ConnetedUserDetail { UserName = Username }, Context.ConnectionId);
        //    var Redis4 = new RadisServices<ConnetedClientList>(this._fact);
        //    Redis4.SaveToHash(Context.ConnectionId, new ConnetedClientList { ConnectionId = Context.ConnectionId }, AccessToken);
        //    //var Redis3 = new RadisServices<BlockUserDetail>(this._fact);
        //    //BlockUserDetail User = new BlockUserDetail();
        //    //User = Redis3.GetConnectionID(UserID + ":BlockDetail");
        //    //if (string.IsNullOrEmpty(User.ToString()) && !User.IsBlock)
        //    //{
        //    //    Groups.AddToGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
        //    //}
        //    //else
        //    //{
        //    //    // on this action Remove text box from Chat for this client
        //    //    _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("BlockedUser", User.ToString());
        //    //}            
        //    //Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast");
        //}

        public Task OnConnected(string Token, string Username)
        {
            // var Redis = new RadisServices<ConnetedClientList>(this._fact);
            // Redis.SaveToHash(Context.ConnectionId, new ConnetedClientList { ConnectionId = Context.ConnectionId }, Token);

            var Redis = new RadisServices<ConnetedClientToken>(this._fact);
            Redis.SaveToHash("Users:" + Context.ConnectionId, new ConnetedClientToken { Token = Token }, Token,Context.ConnectionId);
            return Task.FromResult(0);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //var Redis = new RadisServices<ConnetedClientList>(this._fact);
            //Redis.Scan(Context.ConnectionId, ":ConnectionDetail");
            
            var Redis = new RadisServices<ConnetedClientList>(this._fact);            
            string Pair = Redis.GetPair(Context.ConnectionId,":");
            //GetConnectedClient(Pair);
            Redis.DeleteHash("Users:"+Context.ConnectionId);
            Redis.RemoveSetMember("Pairs:" + Pair, Context.ConnectionId);
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BroadCast").Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
            return base.OnDisconnectedAsync(exception);
        }

        public IReadOnlyList<string> GetConnectedClient(string Pair)
        {
            var Redis = new RadisServices<string>(this._fact);
            IReadOnlyList<string> ConnectedClient = Redis.GetSetList(Pair);
            return ConnectedClient;
        }

        // Remove From Subscription Channel
        public Task RemoveSubscription(string Pair)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
            return Task.FromResult(0);
        }      


        // Add to Subscription Channel
        public Task AddSubscription(string Pair)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.SaveTagsToSetMember("Pairs:" + Pair, Context.ConnectionId, Context.ConnectionId);
            Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
            return Task.FromResult(0);
        }

        // One to one Message Chat
        public Task SendChatMessage(string UserID, string Message)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveMessage", User + ": " + Message);
            //_chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", User + ": " + message);
            return Task.FromResult(0);
        }

        public Task PendingOrderUpdate(string UserID, string Data)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecievePendingOrderUpdate", Data);
            //_chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
            return Task.FromResult(0);
        }     

        public Task SendGroupMessage(string Name, string Message)
        {
            // Call the broadcastMessage method to update _chatHubContext.Clients.
            _chatHubContext.Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", Name, Message);
            var Redis = new RadisServices<ChatHistory>(this._fact);
            Redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = Name, Message = Message, Id = Guid.NewGuid() }, Name);
            Redis.GetSetData("GroupChatHistory");
            return Task.FromResult(0);
        }

        public Task GetChatHistory(string Name, string Message)
        {
            var Redis = new RadisServices<ChatHistory>(this._fact);
            string Data = Redis.GetSetData("GroupChatHistory");
            _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("RecieveChatHistory", Data);
            return Task.FromResult(0);
        }

        //public void getTime(string countryZone)
        public Task GetTime()
        {
            _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("SetTime", Helpers.Helpers.GetUTCTime());
            return Task.FromResult(0);
        }

        //User Specific Updates
        public Task OpenOrder(string Token, string Order)
        {
            //var Redis = new RadisServices<ConnetedClientList>(this._fact);
            //ConnetedClientList User = new ConnetedClientList();
            //User = Redis.GetConnectionID(Token);
            //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOpenOrder", Order);
            var Redis = new RadisServices<ConnetedClientToken>(this._fact);
            IEnumerable<string> str =  Redis.GetKey(Token);
            return Task.FromResult(0);
        }

        public Task OrderHistory(string Token, string Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOrderHistory", Order);
            return Task.FromResult(0);
        }

        public Task TradeHistoryByUser(string Token, string Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveTradeHistory", Order);
            return Task.FromResult(0);
        }
        
        public Task BuyerSideWalletBal(string Token, string Data)
        {
            //var Name= Context.User.Identity.Name;
            //_chatHubContext.Clients.Group("BuyerSideWalletBal:" + Pair).SendAsync("RecieveBuyerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveBuyerSideWalletBal", Data);
            return Task.FromResult(0);
        }

        public Task SellerSideWalletBal(string Token, string Data)
        {
            //_chatHubContext.Clients.Group("SellerSideWalletBal:" + Pair).SendAsync("RecieveSellerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveSellerSideWalletBal", Data);
            return Task.FromResult(0);
        }

        // Global Updates
        public Task BuyerBook(string Pair, string Data)
        {
            //_chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Helpers.Helpers.JsonSerialize(Data));
            _chatHubContext.Clients.Group("BuyerBook:" + Pair).SendAsync("RecieveBuyerBook", Data);
            return Task.FromResult(0);
        }

        // For Demo get Connections Topic wise subscription using Redis // Too SLow for Output 
        public Task BuyerBookWithRedis(string Pair, string Data)
        {
            _chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Data);
            return Task.FromResult(0);
        }

        public Task SellerBook(string Pair, string Data)
        {
            _chatHubContext.Clients.Group("SellerBook:" + Pair).SendAsync("RecieveSellerBook", Data);
            return Task.FromResult(0);
        }

        // Global Trades settelment
        public Task TradeHistoryByPair(string Pair, string Data)
        {
            _chatHubContext.Clients.Group("TradingHistory:" + Pair).SendAsync("RecieveTradingHistory", Data);
            return Task.FromResult(0);
        }

        public Task MarketData(string Pair, string Data)
        {
            _chatHubContext.Clients.Group("MarketData:" + Pair).SendAsync("RecieveMarketData", Data);
            return Task.FromResult(0);
        }        

        public Task ChartData(string Pair, string Data)
        {
            _chatHubContext.Clients.Group("ChartData:" + Pair).SendAsync("RecieveChartData", Data);
            return Task.FromResult(0);
        }

        //public void BlockedUser(string UserID)
        //{
        //    // Add to blocklist
        //    var Redis2 = new RadisServices<BlockUserDetail>(this._fact);
        //    Redis2.SaveToHash(UserID + ":BlockDetail", new BlockUserDetail { IsBlock = true }, Context.ConnectionId);

        //    // Call the broadcastMessage method to update _chatHubContext.Clients.
        //    //_chatHubContext.Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
        //    //var redis = new RadisServices<ChatHistory>(this._fact);
        //    //red-is.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
        //    //redis.GetSetData("GroupChatHistory");
        //}

        //public void TransactionHistory(string Data)
        //{
        //    _chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
        //}
    }
}