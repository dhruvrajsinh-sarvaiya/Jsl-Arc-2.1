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
        private SocketHub _chat;
        public SinalREventHandler( SocketHub chat)
        {
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
                else if (request.Method == enMethodName.OrderHistory)
                    _chat.OrderHistory(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.ChartData)
                    _chat.ChartData(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.MarketData)
                    _chat.MarketData(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.OpenOrder )
                    _chat.OpenOrder(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.TradeHistory)
                    _chat.TradeHistory(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.RecentOrder)
                    _chat.RecentOrder(request.Parameter, request.DataObj);
                else if (request.Method == enMethodName.BuyerSideWallet)
                    _chat.BuyerSideWalletBal(request.Parameter, request.WalletName, request.DataObj);
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
                    _chat.BroadCastNews(request.DataObj);
                else if (request.Method == enMethodName.Announcement)
                    _chat.BroadCastAnnouncement(request.DataObj);
                else if (request.Method == enMethodName.SendGroupMessage)
                    _chat.SendGroupMessage(request.Parameter,request.DataObj);
                else if (request.Method == enMethodName.Time)
                    _chat.GetTime(request.DataObj);

                return Task.FromResult(new Unit());
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Unit());
            }
        }
    }
}
