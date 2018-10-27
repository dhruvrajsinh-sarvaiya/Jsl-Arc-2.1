using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetBuySellBookResponse : BizResponseClass
    {
      public List<GetBuySellBook> response { get; set; }
    }
    public class GetBuySellBook
    {
        public Decimal Amount { get; set; }
        public Decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public int RecordCount { get; set; }
    }
}
