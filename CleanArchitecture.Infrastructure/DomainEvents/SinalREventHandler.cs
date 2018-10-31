using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SignalR;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CleanArchitecture.Infrastructure.DomainEvents
{
    public class SinalREventHandler : IRequestHandler<SignalRData>
    {
        private readonly IMessageRepository<NotificationQueue> _MessageRepository;
        private readonly IMessageConfiguration _MessageConfiguration;
        private readonly IMessageService _MessageService;
        private WebApiParseResponse _WebApiParseResponse;
        private GetDataForParsingAPI _GetDataForParsingAPI;
        private WebAPIParseResponseCls _GenerateResponse;
        //private readonly IHubContext<Chat> _hubContext;
        private SocketHub _chat;


        public SinalREventHandler(IMessageRepository<NotificationQueue> MessageRepository, 
            MessageConfiguration MessageConfiguration, 
            MessageService MessageService, GetDataForParsingAPI GetDataForParsingAPI, 
            WebApiParseResponse WebApiParseResponse, WebAPIParseResponseCls GenerateResponse, SocketHub chat)
        {
            _MessageRepository = MessageRepository;
            _MessageConfiguration = MessageConfiguration;
            _MessageService = MessageService;
            _GetDataForParsingAPI = GetDataForParsingAPI;
            _WebApiParseResponse = WebApiParseResponse;
            _GenerateResponse = GenerateResponse;
            _chat = chat;
        }
        
        public Task<Unit> Handle(SignalRData request, CancellationToken cancellationToken)
        {
            try
            {
                
                if (request.Method == enMethodName.BuyerBook)
                    _chat.BuyerBook(request.Parameter,request.DataObj);
                else if (request.Method == enMethodName.SellerBook)
                    _chat.SellerBook(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.TradeHistoryByPair)
                    _chat.TradeHistoryByPair(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.ChartData)
                    _chat.ChartData(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.MarketData)
                    _chat.MarketData(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.OpenOrder )
                    _chat.OpenOrder(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.OrderHistory)
                    _chat.OrderHistory(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.TradeHistory)
                    _chat.TradeHistoryByUser(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.BuyerSideWallet)
                    _chat.BuyerSideWalletBal(request.Parameter,request.WalletName, request.DataObj);
                else if (request.Method == enMethodName.SellerSideWallet)
                    _chat.SellerSideWalletBal(request.Parameter, request.WalletName, request.DataObj);
                else if (request.Method == enMethodName.Price)
                    _chat.LastPrice(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.PairData)
                    _chat.PairData(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.MarketTicker)
                    _chat.MarketTicker(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.ActivityNotification)
                    _chat.ActivityNotification(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.News)
                    _chat.BroadCastData(request.DataObj);
                else if (request.Method == enMethodName.Announcement)
                    _chat.BroadCastData(request.DataObj);

                //_chat.OpenOrder("CfDJ8MB8DXV9k79JrEe_PIsRHuriLlB4BgdnHH8a68GFLRS8lKmDBmEroQPEr7Ut3SdC4N3B6LKb826fWnx5CCHwGA28z0Qj2_mU9jp3L2HgggdUTI38vyxTUNrDKAo_ajk7Mn8D9-PdbcXYqGIVNSUZ58Vj0XqbW9nAkKcfIx6ZfSy9JY_w5aBtVL8mLB3gqKuX-SWyMsEGKzW70_cU49ALKLEUo1p6Pap4S4d3QCbnbiBV5dw4sshE5DhXdPflobrF6G1kDBzCyL8nLr3GFnJZIQPQfKl8v80MuLtBDmDttfAkNP_vbL0dg-muZipMLFCvkz1Sg3pPGbcCTaU-u1EsrRw3CB8LKR3yfpDGOUkr0mj3mU7rNE15hePJnUZPSeAiniKnouXmvkg5bTp4LjVUWY_uRKk2SJNYjBDfw68UtpyhTD_FjgVNL7pqW0GNoGCFUXwYz9JkxtR00aFo8xuH4pIOSmVqWIM-UXHMRQwbwML_HL0rZ79Ol11w-HuIwz5NPJ40vb5sFPlD54FkH5F522a3YmT8GBkYltGJfnpfz_BqHKw1JFO2RuVhpTqONfibcp8hyhP8alB0Eaok12xuSI0km8PAd8ZKqCu3O-gqOsn6e8pu9ZRxSZgsL-VB2kTMmtKsc2TB74m8Cus15T6yB0-vTBju1Vl6sptcoRo-bS8tgZ858aw9MJOZcU0FlFMMICfJAwpQNdpSBQWQ8eAF49Z8cc7_KUGBen93ZYrKsIhdZ0tvTl_5MKkbbTDYOFWri9VzyarzpTPlh7l8uYXN1peJOdARroPSF3MF7kPyiEXjnERRtzD02GfwjPujDLtlw8wqNL9aONYwBwertKVUYOs", "xyz");
                return Task.FromResult(new Unit());
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Unit());
            }
        }
    }
}
