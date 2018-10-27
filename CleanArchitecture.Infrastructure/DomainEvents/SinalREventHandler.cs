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
                
                if(request.Method == enPairWiseMethodName.BuyerBook)
                    _chat.BuyerBook(request.Parameter, request.Data);
                else if (request.Method == enPairWiseMethodName.SellerBook)
                    _chat.SellerBook(request.Parameter, request.Data);
                else if(request.Method == enPairWiseMethodName.TradeHistoryByPair)
                    _chat.TradeHistoryByPair(request.Parameter, request.Data);
                else if (request.Method == enPairWiseMethodName.ChartData)
                    _chat.ChartData(request.Parameter, request.Data);
                else if (request.Method == enPairWiseMethodName.MarketData)
                    _chat.MarketData(request.Parameter, request.Data);

                return Task.FromResult(new Unit());
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Unit());
            }
        }
    }
}
