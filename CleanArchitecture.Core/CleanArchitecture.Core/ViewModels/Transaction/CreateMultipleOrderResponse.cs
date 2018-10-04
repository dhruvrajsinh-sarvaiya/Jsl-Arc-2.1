using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateMultipleOrderResponse : BizResponseClass
    {
        public List<CreateOrderInfo> response { get; set; }
    }
}
