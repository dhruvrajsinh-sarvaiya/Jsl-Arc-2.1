using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public  class GetRecentTradeRequest
    {
        [Required]
        public string Pair { get; set; }

        public int Limit { get; set; }
    }
}
