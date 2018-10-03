using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CancelOrderResponse
    {
        public long order_id { get; set; }
        public string message { get; set; }
    }
}
