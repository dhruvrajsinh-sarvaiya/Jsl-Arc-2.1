using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetActiveOrderResponse : BizResponseClass
    {
        public List<ActiveOrderInfo> response { get; set; }
    }
    public class  ActiveOrderInfo
    {
        public long Id { get; set; }
        public DateTime  TrnDate { get; set; }
        public short Type { get; set; }
        public string Order_Currency { get; set; }
        public string Delivery_Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public short IsCancelled { get; set; }
    }
    
}
