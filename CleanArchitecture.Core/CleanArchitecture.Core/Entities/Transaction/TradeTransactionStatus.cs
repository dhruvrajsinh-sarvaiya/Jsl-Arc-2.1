﻿using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradeTransactionStatus :BizBase
    {
        public long TrnNo { get; set; }

        [Required ]
        [Range(0, 9999999999.99999999)]
        public decimal SettledQty { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal TotalQty { get; set; }
        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal DeliveredQty { get; set; }
        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal PendingQty { get; set; }
        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal SoldPrice { get; set; }
        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal BidPrice { get; set; }

        [Required]
        public long OrderID { get; set; }
        [Required]
        public long StockID { get; set; }
        public long? SellStockID { get; set; }
    }
}