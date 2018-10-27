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
        public void SendToAll(string name, string message)
        {
            _chatHubContext.Clients.All.SendAsync("sendToAll", name, message);
        }

        //For Testing only
        public void SattledOrderUpdate(string Data)
        {
            _chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
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

        public void OnConnected(string Token, string Username)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.SaveToHash(Context.ConnectionId, new ConnetedClientList { ConnectionId = Context.ConnectionId }, Token);
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            //var Redis = new RadisServices<ConnetedClientList>(this._fact);
            //Redis.Scan(Context.ConnectionId, ":ConnectionDetail");
            
            var Redis = new RadisServices<ConnetedClientList>(this._fact);            
            string Pair = Redis.GetPair(Context.ConnectionId,":");
            GetConnectedClient(Pair);
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

        // Remove From Subscription Channel
        public void RemoveSubscription(string Pair)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
        }

        public IReadOnlyList<string> GetConnectedClient(string Pair)
        {
            var Redis = new RadisServices<string>(this._fact);
            IReadOnlyList<string> ConnectedClient = Redis.GetSetList(Pair);
            return ConnectedClient;
        }


        // Add to Subscription Channel
        public void AddSubscription(string Pair)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            Redis.SaveTagsToSetMember("Pairs:" + Pair, Context.ConnectionId, Context.ConnectionId);
            Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
            Groups.AddToGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
        }

        // One to one Message Chat
        public void SendChatMessage(string UserID, string Message)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveMessage", User + ": " + Message);
            //_chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", User + ": " + message);
        }

        public void PendingOrderUpdate(string UserID, string Data)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(UserID + ":ConnectionDetail");
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecievePendingOrderUpdate", Data);
            //_chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
        }     

        public void SendGroupMessage(string Name, string Message)
        {
            // Call the broadcastMessage method to update _chatHubContext.Clients.
            _chatHubContext.Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", Name, Message);
            var Redis = new RadisServices<ChatHistory>(this._fact);
            Redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = Name, Message = Message, Id = Guid.NewGuid() }, Name);
            Redis.GetSetData("GroupChatHistory");
        }

        public void GetChatHistory(string Name, string Message)
        {
            var Redis = new RadisServices<ChatHistory>(this._fact);
            string Data = Redis.GetSetData("GroupChatHistory");
            _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("RecieveChatHistory", Data);
        }

        //public void getTime(string countryZone)
        public void GetTime()
        {
            TimeZone currentZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC = currentZone.ToUniversalTime(currentDate);
            //TimeZoneInfo selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(countryZone);
            //DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUTC, selectedTimeZone);
            //_chatHubContext.Clients.Caller.setTime(currentDateTime.ToString("h:mm:ss tt"));
            //_chatHubContext.Clients.Caller.SendAsync("SetTime",currentUTC.ToLongTimeString());
            _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("SetTime", Convert.ToInt64((currentUTC - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds*1000).ToString());
        }

        //User Specific Updates
        public void OpenOrder(string Token, ActiveOrderInfo Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOpenOrder", Helpers.Helpers.JsonSerialize(Order));
        }

        public void OrderHistory(string Token, GetTradeHistoryResponse Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOrderHistory", Helpers.Helpers.JsonSerialize(Order));
        }

        public void TradeHistory(string Token, GetTradeHistoryResponse Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveTradeHistory", Helpers.Helpers.JsonSerialize(Order));
        }
        
        public void BuyerSideWalletBal(string Token, BalanceResponse Data)
        {
            //var Name= Context.User.Identity.Name;
            //_chatHubContext.Clients.Group("BuyerSideWalletBal:" + Pair).SendAsync("RecieveBuyerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveBuyerSideWalletBal", Helpers.Helpers.JsonSerialize(Data));
        }

        public void SellerSideWalletBal(string Token, BalanceResponse Data)
        {
            //_chatHubContext.Clients.Group("SellerSideWalletBal:" + Pair).SendAsync("RecieveSellerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveSellerSideWalletBal", Helpers.Helpers.JsonSerialize(Data));
        }

        // Global Updates
        public void BuyerBook(string Pair, GetBuySellBook Data)
        {
            _chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Helpers.Helpers.JsonSerialize(Data));
            _chatHubContext.Clients.Group("BuyerBook:" + Pair).SendAsync("RecieveBuyerBook", Helpers.Helpers.JsonSerialize(Data));
        }

        // For Demo get Connections Topic wise subscription using Redis // Too SLow for Output 
        public void BuyerBookWithRedis(string Pair, GetBuySellBook Data)
        {
            _chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Helpers.Helpers.JsonSerialize(Data));
        }

        public void SellerBook(string Pair, GetBuySellBook Data)
        {
            Clients.Group("SellerBook:" + Pair).SendAsync("RecieveSellerBook", Helpers.Helpers.JsonSerialize(Data));
        }

        // Global Trades settelment
        public void TradingHistory(string Pair, TradeHistoryResponce Data)
        {
            _chatHubContext.Clients.Group("TradingHistory:" + Pair).SendAsync("RecieveTradingHistory", Helpers.Helpers.JsonSerialize(Data));
        }

        public void MarketData(string Pair, MarketCapData Data)
        {
            _chatHubContext.Clients.Group("MarketData:" + Pair).SendAsync("RecieveMarketData", Helpers.Helpers.JsonSerialize(Data));
        }        

        public void ChartData(string Pair, GetGraphResponse Data)
        {
            _chatHubContext.Clients.Group("ChartData:" + Pair).SendAsync("RecieveChartData", Helpers.Helpers.JsonSerialize(Data));
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