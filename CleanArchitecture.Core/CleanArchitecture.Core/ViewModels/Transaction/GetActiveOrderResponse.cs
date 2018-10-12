using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetActiveOrderResponse : BizResponseClass
    {
       public List<GetActiveOrderInfo> response { get; set; }
    }

    public class GetActiveOrderInfo
    {
        public long OrderId { get; set; }
        public string PairName { get; set; }
        public string BaseCurrency { get; set; }
        public string ChildCurrency { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime TrnDate { get; set; }
        public bool IsCancel { get; set; }
    }
}
