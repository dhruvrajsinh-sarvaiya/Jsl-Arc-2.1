using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MyTradesFundingRequest
    {
        [Required]
        public string Symbol { get; set; }

        public string until { get; set; }

        public Int32 limit_trades { get; set; }
    }

}
