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
        //    //    Clients.Client(Context.ConnectionId).SendAsync("BlockedUser", User.ToString());
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

        //public void getTime(string countryZone)
        public void GetTime()
        {
            TimeZone currentZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            DateTime currentUTC = currentZone.ToUniversalTime(currentDate);
            //TimeZoneInfo selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(countryZone);
            //DateTime currentDateTime = TimeZoneInfo.ConvertTimeFromUtc(currentUTC, selectedTimeZone);
            //Clients.Caller.setTime(currentDateTime.ToString("h:mm:ss tt"));
            //Clients.Caller.SendAsync("SetTime",currentUTC.ToLongTimeString());
            Clients.Caller.SendAsync("SetTime", Convert.ToInt64((currentUTC - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds*1000).ToString());
        }

        //User Specific Updates
        public void OpenOrder(string Token, ActiveOrderInfo Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            Clients.Client(User.ConnectionId).SendAsync("RecieveOpenOrder", Helpers.JsonSerialize(Order));
        }

        public void OrderHistory(string Token, GetTradeHistoryResponse Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            Clients.Client(User.ConnectionId).SendAsync("RecieveOrderHistory", Helpers.JsonSerialize(Order));
        }

        public void TradeHistory(string Token, GetTradeHistoryResponse Order)
        {
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            Clients.Client(User.ConnectionId).SendAsync("RecieveTradeHistory", Helpers.JsonSerialize(Order));
        }
        
        public void BuyerSideWalletBal(string Token, BalanceResponse Data)
        {
            //var Name= Context.User.Identity.Name;
            //Clients.Group("BuyerSideWalletBal:" + Pair).SendAsync("RecieveBuyerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            Clients.Client(User.ConnectionId).SendAsync("RecieveBuyerSideWalletBal", Helpers.JsonSerialize(Data));
        }

        public void SellerSideWalletBal(string Token, BalanceResponse Data)
        {
            //Clients.Group("SellerSideWalletBal:" + Pair).SendAsync("RecieveSellerSideWalletBal", Data);
            var Redis = new RadisServices<ConnetedClientList>(this._fact);
            ConnetedClientList User = new ConnetedClientList();
            User = Redis.GetConnectionID(Token);
            Clients.Client(User.ConnectionId).SendAsync("RecieveSellerSideWalletBal", Helpers.JsonSerialize(Data));
        }

        // Global Updates
        public void BuyerBook(string Pair, GetBuySellBook Data)
        {
            Clients.Group("BuyerBook:" + Pair).SendAsync("RecieveBuyerBook", Data);
        }

        public void SellerBook(string Pair, GetBuySellBook Data)
        {
            Clients.Group("SellerBook:" + Pair).SendAsync("RecieveSellerBook", Data);
        }

        // Global Trades settelment
        public void TradingHistory(string Pair, TradeHistoryResponce Data)
        {
            Clients.Group("TradingHistory:" + Pair).SendAsync("RecieveTradingHistory", Data);
        }

        public void MarketData(string Pair, MarketCapData Data)
        {
            Clients.Group("MarketData:" + Pair).SendAsync("RecieveMarketData", Data);
        }        

        public void ChartData(string Pair, GetGraphResponse Data)
        {
            Clients.Group("ChartData:" + Pair).SendAsync("RecieveChartData", Data);
        }

        public void BlockedUser(string UserID)
        {
            // Add to blocklist
            var Redis2 = new RadisServices<BlockUserDetail>(this._fact);
            Redis2.SaveToHash(UserID + ":BlockDetail", new BlockUserDetail { IsBlock = true }, Context.ConnectionId);

            // Call the broadcastMessage method to update clients.
            //Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
            //var redis = new RadisServices<ChatHistory>(this._fact);
            //red-is.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
            //redis.GetSetData("GroupChatHistory");
        }       

    }
}