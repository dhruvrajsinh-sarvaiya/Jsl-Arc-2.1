using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class TradeLedgerRequest
    {
        [Required]
        [StringLength (7,MinimumLength =6)]
        public string Pair { get; set; }

        [Required]
        public long time { get; set; }
        public long StartTime { get; set; }
        public long EndTime { get; set; }
        public long TradeId { get; set; }

    }
}
