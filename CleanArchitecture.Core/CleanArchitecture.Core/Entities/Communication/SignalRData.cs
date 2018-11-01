using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SignalR;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Communication
{    
    public class SignalRData: IRequest
    {
        public string DataObj { get; set; }
        public enMethodName Method { get; set; }
        public string Parameter { get; set; }
        public string WalletName { get; set; }
    }
    public class TradeHistoryInfo
    {
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Amount { get; set; }
        public Decimal Total { get; set; }
        public DateTime DateTime { get; set; }
        public short Status { get; set; }
        public string StatusText { get; set; }
        public String PairName { get; set; }
        public Decimal ChargeRs { get; set; }
        public short IsCancel { get; set; }
        //public string OrderType { get; set; }
    }
}
