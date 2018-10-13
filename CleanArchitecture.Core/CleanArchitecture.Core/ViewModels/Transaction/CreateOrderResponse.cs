using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateTransactionResponse : BizResponseClass
    { 
        public CreateOrderInfo response { get; set; }
    }
    public class CreateOrderInfo
    {
        //public long TrnNo { get; set; }      
        public Guid TrnID { get; set; }
        [DataMember]
        public RateInfo RateInfo { get; set; }
    }
    public class RateInfo
    {
        [DataMember]
        public decimal SellRate { get; set; }
        [DataMember]
        public decimal BuyRate { get; set; }
        [DataMember]
        public decimal Buy_Min_Price { get; set; }
        [DataMember]
        public decimal Buy_Max_Price { get; set; }
        [DataMember]
        public decimal Sell_Min_Price { get; set; }
        [DataMember]
        public decimal Sell_Max_Price { get; set; }
    }
}
