using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CancelMultipleOrderResponse : BizResponseClass
    {
        public List<CancelOrderInfo> response { get; set; }
    }
}
