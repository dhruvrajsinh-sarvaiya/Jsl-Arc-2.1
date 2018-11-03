using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using CleanArchitecture.Core.Services.RadisDatabase;
using CleanArchitecture.Core.ApiModels.Chat;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.SignalR
{
    public class SocketHub : Hub
    {
        private RedisConnectionFactory _fact;
        private IHubContext<SocketHub> _chatHubContext;
        private readonly ILogger<SocketHub> _logger;
        public SocketHub(IHubContext<SocketHub> ChatHubContext,RedisConnectionFactory Factory, ILogger<SocketHub> logger)
        {            
            _fact = Factory;
            _chatHubContext = ChatHubContext;
            _logger = logger;
        }

        #region "For Testing Connection"
        public Task SendToAll(string name, string message)
        {
            try
            {
                _chatHubContext.Clients.All.SendAsync("sendToAll", name, message);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }
        #endregion

        #region "For Connection Management By default from Hub Class"

        public override Task OnConnectedAsync()
        {
            try
            {
                string Pair = "INR_BTC";
                string BaseCurrency = "BTC";
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                Redis.SaveTagsToSetMember("Pairs:" + Pair, Context.ConnectionId, Pair); 
                Redis.SaveTagsToSetMember("Markets:" + BaseCurrency, Context.ConnectionId, BaseCurrency);
                Groups.AddToGroupAsync(Context.ConnectionId, "BroadCast").Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "GroupMessage").Wait();               
                Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();            
                Groups.AddToGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "Price:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "PairData:" + BaseCurrency).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "MarketTicker:" + BaseCurrency).Wait();
                return base.OnConnectedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
            
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

        //public Task OnConnected(string Token, string Username)
        //{
        //    try
        //    {
        //        // var Redis = new RadisServices<ConnetedClientList>(this._fact);
        //        // Redis.SaveToHash(Context.ConnectionId, new ConnetedClientList { ConnectionId = Context.ConnectionId }, Token);

        //        var Redis = new RadisServices<ConnetedClientToken>(this._fact);
        //        Redis.SaveToHash("Users:" + Context.ConnectionId, new ConnetedClientToken { Token = Token }, Token,Context.ConnectionId);
        //        return Task.FromResult(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        return Task.FromResult(0);
        //    }

        //}

        //public Task OnTokenChange(string NewToken,string OldToken)
        //{
        //    try
        //    {
        //        var Redis = new RadisServices<ConnetedClientToken>(this._fact);
        //        Redis.DeleteTag("Users:" + Context.ConnectionId, OldToken);
        //        Redis.SaveToHash("Users:" + Context.ConnectionId, new ConnetedClientToken { Token = NewToken }, NewToken);
        //        return Task.FromResult(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        return Task.FromResult(0);
        //    }            
        //}

        public Task OnConnected(string Token)
        {
            try
            {
                // var Redis = new RadisServices<ConnetedClientList>(this._fact);
                // Redis.SaveToHash(Context.ConnectionId, new ConnetedClientList { ConnectionId = Context.ConnectionId }, Token);

                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                string AccessToken = Redis.GetHashData(Token, "accessToken");
                Redis.SaveToHash("Users:" + Context.ConnectionId, new ConnetedClientToken { Token = AccessToken }, AccessToken);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }

        }

        public Task OnTokenChange(string Token)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                string AccessToken = Redis.GetHashData(Token, "accessToken");
                ConnetedClientToken ClientToken = new ConnetedClientToken();
                ClientToken = Redis.GetData("Users:" + Context.ConnectionId);
                Redis.DeleteTag("Users:" + Context.ConnectionId, ClientToken.Token);
                Redis.SaveToHash("Users:" + Context.ConnectionId, new ConnetedClientToken { Token = AccessToken }, AccessToken);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
        }

        public override Task OnDisconnectedAsync(System.Exception exception)
        {
            try
            {
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //Redis.Scan(Context.ConnectionId, ":ConnectionDetail");

                var Redis = new RadisServices<ConnetedClientToken>(this._fact);            
                string Pair = Redis.GetPairOrMarketData(Context.ConnectionId,":", "Pairs");
                string BaseCurrency = Redis.GetPairOrMarketData(Context.ConnectionId,":", "Markets");
                //GetConnectedClient(Pair);
                ConnetedClientToken Client = new ConnetedClientToken();
                Client = Redis.GetData("Users:" + Context.ConnectionId);
                if (Client != null)
                {
                    Redis.DeleteTag("Users:" + Context.ConnectionId, Client.Token);
                }
                Redis.DeleteTag("Pairs:" + Pair, Context.ConnectionId);
                Redis.DeleteTag("Markets:" + BaseCurrency, Context.ConnectionId);
                Redis.DeleteHash("Users:"+Context.ConnectionId);
                Redis.RemoveSetMember("Pairs:" + Pair, Context.ConnectionId);
                Redis.RemoveSetMember("Markets:" + BaseCurrency, Context.ConnectionId);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "Price:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "BroadCast").Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "GroupMessage").Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "PairData:" + BaseCurrency).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketTicker:" + BaseCurrency).Wait();
                return base.OnDisconnectedAsync(exception);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public IReadOnlyList<string> GetConnectedClient(string Pair)
        {
            try
            {
                var Redis = new RadisServices<string>(this._fact);
                IReadOnlyList<string> ConnectedClient = Redis.GetSetList(Pair);
                return ConnectedClient;
            }
            catch (Exception ex)
            {
                List<string> ConnectedClient = new List<string>();
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return ConnectedClient.AsReadOnly();
            }
            
        }

        #endregion

        #region "Subscription Managemnet"

        // Remove From Subscription Channel
        public Task RemovePairSubscription(string Pair)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                Redis.DeleteTag("Pairs:" + Pair, Context.ConnectionId);
                Redis.RemoveSetMember("Pairs:" + Pair, Context.ConnectionId);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "Price:" + Pair).Wait();
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }
        
        // Add to Subscription Channel
        public Task AddPairSubscription(string Pair,string OldPair)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                Redis.SaveTagsToSetMember("Pairs:" + Pair, Context.ConnectionId, Context.ConnectionId);
                Groups.AddToGroupAsync(Context.ConnectionId, "BuyerBook:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "SellerBook:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "TradingHistory:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "MarketData:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "ChartData:" + Pair).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "Price:" + Pair).Wait();
                RemovePairSubscription(OldPair);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        // Remove From Subscription Channel
        public Task RemoveMarketSubscription(string BaseCurrency)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                Redis.DeleteTag("Markets:" + BaseCurrency, Context.ConnectionId);
                Redis.RemoveSetMember("Markets:" + BaseCurrency, Context.ConnectionId);
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "PairData:" + BaseCurrency).Wait();
                Groups.RemoveFromGroupAsync(Context.ConnectionId, "MarketTicker:" + BaseCurrency).Wait();
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        // Add to Subscription Channel
        public Task AddMarketSubscription(string BaseCurrency,string OldBaseCurrency)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                Redis.SaveTagsToSetMember("Markets:" + BaseCurrency, Context.ConnectionId, Context.ConnectionId);
                Groups.AddToGroupAsync(Context.ConnectionId, "PairData:" + BaseCurrency).Wait();
                Groups.AddToGroupAsync(Context.ConnectionId, "MarketTicker:" + BaseCurrency).Wait();
                RemoveMarketSubscription(OldBaseCurrency);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        #endregion

        #region "Chat Module"

        // One to one Message Chat
        public Task SendChatMessage(string UserID, string Message)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientList>(this._fact);
                ConnetedClientList User = new ConnetedClientList();
                User = Redis.GetData(UserID + ":ConnectionDetail");
                _chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveMessage", User + ": " + Message);
                //_chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", User + ": " + message);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }           
        }     

        public Task SendGroupMessage(string Name, string Message)
        {
            try
            {
                // Call the broadcastMessage method to update _chatHubContext.Clients.
                _chatHubContext.Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", Name, Message);
                var Redis = new RadisServices<ChatHistory>(this._fact);
                Redis.SaveToSet("GroupChatHistory", new ChatHistory { Name = Name, Message = Message, Id = Guid.NewGuid() }, Name);
                Redis.GetSetData("GroupChatHistory");
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task GetChatHistory(string Name, string Message)
        {
            try
            {
                var Redis = new RadisServices<ChatHistory>(this._fact);
                string Data = Redis.GetSetData("GroupChatHistory");
                _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("RecieveChatHistory", Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        #endregion

        #region "Global Updates For Time , News , Announce"

        //public void getTime(string countryZone)
        public Task GetTime()
        {
            try
            {
                _chatHubContext.Clients.Client(Context.ConnectionId).SendAsync("SetTime", Helpers.Helpers.GetUTCTime());
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task BroadCastData(string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("BroadCast").SendAsync("BroadcastMessage", Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task BroadCastNews(string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("BroadCast").SendAsync("RecieveNews", Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
        }

        public Task BroadCastAnnouncement(string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("BroadCast").SendAsync("RecieveAnnouncement", Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
        }

        #endregion

        #region "User Specific Updates"
        //open order
        public Task OpenOrder(string Token, string Order)
        {
            try
            {
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //ConnetedClientList User = new ConnetedClientList();
                //User = Redis.GetConnectionID(Token);
                //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOpenOrder", Order);
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> str =  Redis.GetKey(Token);
                foreach(string s in str.ToList())
                {
                    var key = s;
                    key = key.Split(":")[1].ToString();
                    _chatHubContext.Clients.Client(key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOpenOrder), Order);
                }
                // _chatHubContext.Clients.Client(str.ToList().AsReadOnly()).SendAsync("RecieveOpenOrder", Order);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }
        //OrderHistory
        public Task TradeHistory(string Token, string Order)
        {
            try
            {
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //ConnetedClientList User = new ConnetedClientList();
                //User = Redis.GetConnectionID(Token);
                //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveOrderHistory", Order);
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> str = Redis.GetKey(Token);
                foreach (string s in str.ToList())
                {
                    var key = s;
                    key = key.Split(":")[1].ToString();
                    _chatHubContext.Clients.Client(key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveTradeHistory), Order);
                }
                return Task.FromResult(0);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }           
        }
        //TradeHistoryByUser
        public Task RecentOrder(string Token, string Order)
        {
            try
            {
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //ConnetedClientList User = new ConnetedClientList();
                //User = Redis.GetConnectionID(Token);
                //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveTradeHistory", Order);
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> str = Redis.GetKey(Token);
                foreach (string s in str.ToList())
                {
                    var key = s;
                    key = key.Split(":")[1].ToString();
                    _chatHubContext.Clients.Client(key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveRecentOrder), Order);
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }
        
        public Task BuyerSideWalletBal(string Token, string WalletName, string Data)
        {
            try
            {
                //var Name= Context.User.Identity.Name;
                //_chatHubContext.Clients.Group("BuyerSideWalletBal:" + Pair).SendAsync("RecieveBuyerSideWalletBal", Data);
                //++++++++++++++++++++++//
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //ConnetedClientList User = new ConnetedClientList();
                //User = Redis.GetConnectionID(Token);
                //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveBuyerSideWalletBal", Data);
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> ClientList = Redis.GetKey(Token);
                foreach (string s in ClientList.ToList())
                {
                    var Key = s;
                    Key = Key.Split(":")[1].ToString();
                    string Pair = Redis.GetPairOrMarketData(Key, ":", "Pairs");
                    if (Pair.ToUpper().Contains(WalletName.ToUpper()))
                    {
                        _chatHubContext.Clients.Client(Key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerSideWalletBal), Data);
                    }
                    else
                    {
                        // ignore Data
                    }                   
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task SellerSideWalletBal(string Token, string WalletName, string Data)
        {
            try
            {
                //_chatHubContext.Clients.Group("SellerSideWalletBal:" + Pair).SendAsync("RecieveSellerSideWalletBal", Data);
                //++++++++++++++++++++++//
                //var Redis = new RadisServices<ConnetedClientList>(this._fact);
                //ConnetedClientList User = new ConnetedClientList();
                //User = Redis.GetConnectionID(Token);
                //_chatHubContext.Clients.Client(User.ConnectionId).SendAsync("RecieveSellerSideWalletBal", Data);
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> ClientList = Redis.GetKey(Token);
                foreach (string s in ClientList.ToList())
                {
                    var Key = s;
                    Key = Key.Split(":")[1].ToString();
                    string Pair = Redis.GetPairOrMarketData(Key, ":", "Pairs");
                    if (Pair.ToUpper().Contains(WalletName.ToUpper()))
                    {
                        _chatHubContext.Clients.Client(Key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerSideWalletBal), Data);
                    }
                    else
                    {
                        // ignore Data
                    }                   
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
            
        }

        //public Task WalletBalChange(string Token, string WalletName, string Data)
        //{
        //    try
        //    { 
        //        var Redis = new RadisServices<ConnetedClientToken>(this._fact);
        //        IEnumerable<string> ClientList = Redis.GetKey(Token);
        //        foreach (string s in ClientList.ToList())
        //        {
        //            var Key = s;
        //            Key = Key.Split(":")[1].ToString();
        //            string Pair = Redis.GetPairOrMarketData(Key, ":", "Pairs");
        //            if (Pair.Split("_")[0].ToString() == WalletName)
        //            {
        //                SellerSideWalletBal(Token,WalletName,Data);
        //            }
        //            else if(Key.Split("_")[1].ToString() == WalletName)
        //            {
        //                BuyerSideWalletBal(Token, WalletName, Data);
        //            }


        //        }
        //        return Task.FromResult(0);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        return Task.FromResult(0);
        //    }
        //}
        public Task ActivityNotification(string Token, string Message)
        {
            try
            {
                var Redis = new RadisServices<ConnetedClientToken>(this._fact);
                IEnumerable<string> str = Redis.GetKey(Token);
                foreach (string s in str.ToList())
                {
                    var key = s;
                    key = key.Split(":")[1].ToString();
                    _chatHubContext.Clients.Client(key).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveNotification), Message);
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
        }

        #endregion

        #region "Pair Wise Global Updates"

        public Task BuyerBook(string Pair, string Data)
        {
            try
            {
                //_chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Helpers.Helpers.JsonSerialize(Data));
                _chatHubContext.Clients.Group("BuyerBook:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveBuyerBook), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        // For Demo get Connections Topic wise subscription using Redis // Too SLow for Output 
        public Task BuyerBookWithRedis(string Pair, string Data)
        {
            try
            {
                if (GetConnectedClient(Pair).Count > 0)
                {
                    _chatHubContext.Clients.Clients(GetConnectedClient(Pair)).SendAsync("RecieveBuyerBook", Data);
                }
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task SellerBook(string Pair, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("SellerBook:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveSellerBook), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
            
        }

        // Global Trades settelment
        //TradeHistoryByPair
        public Task OrderHistory(string Pair, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("TradingHistory:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveOrderHistory), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task MarketData(string Pair, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("MarketData:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveMarketData), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        public Task LastPrice(string Pair, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("Price:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveLastPrice), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }
        }

        public Task ChartData(string Pair, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("ChartData:" + Pair).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecieveChartData), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        #endregion

        #region "Base Market"

        public Task PairData(string BaseCurrency, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("PairData:" + BaseCurrency).SendAsync(Enum.GetName(typeof(enReturnMethod), enReturnMethod.RecievePairData), Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }
        
        public Task MarketTicker(string BaseCurrency, string Data)
        {
            try
            {
                _chatHubContext.Clients.Group("MarketTicker:" + BaseCurrency).SendAsync("RecieveMarketTicker", Data);
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return Task.FromResult(0);
            }            
        }

        #endregion

        //public void BlockedUser(string UserID)
        //{
        //    // Add to blocklist
        //    var Redis2 = new RadisServices<BlockUserDetail>(this._fact);
        //    Redis2.SaveToHash(UserID + ":BlockDetail", new BlockUserDetail { IsBlock = true }, Context.ConnectionId)
        //    // Call the broadcastMessage method to update _chatHubContext.Clients.
        //    //_chatHubContext.Clients.Group("GroupMessage").SendAsync("ReciveGroupMessage", name, message);
        //    //var redis = new RadisServices<ChatHistory>(this._fact);
        //    //red-is.SaveToSet("GroupChatHistory", new ChatHistory { Name = name, Message = message, Id = Guid.NewGuid() }, name);
        //    //redis.GetSetData("GroupChatHistory");
        //}
    }
}