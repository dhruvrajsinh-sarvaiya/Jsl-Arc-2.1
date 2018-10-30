using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetBuySellBookResponse : BizResponseClass
    {
      public List<GetBuySellBook> response { get; set; }
    }
    public class GetBuySellBook
    {
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public Decimal Amount { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public Decimal Price { get; set; }
        public Guid OrderId { get; set; }
        public int RecordCount { get; set; }
    }
}
