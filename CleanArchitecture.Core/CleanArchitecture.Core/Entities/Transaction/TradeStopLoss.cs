using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeStopLoss : BizBase
    {      
        public long TrnNo { get; set; }

        public short ordertype { get; set; } //type of enTransactionMarketType

        //for Maket type Stop-Limit
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal StopPrice { get; set; }
    }
}
