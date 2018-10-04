using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CancelOrderResponse : BizResponseClass
    {
        public CancelOrderInfo response { get; set; }
    }
    public class CancelOrderInfo
    {
        public long order_id { get; set; }
        public string message { get; set; }
    }
}
