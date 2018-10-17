using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string Type { get; set; }
        public string Order_Currency { get; set; }
        public string Delivery_Currency { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public short IsCancelled { get; set; }
    }
    public class GetActiveOrderRequest
    {
        [Required]
        public string Pair { get; set; }

        public string OrderType { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public int Page { get; set; }
    }
}
