using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetTradeHistoryRequest
    {
        [Required]
        public string pair_name { get; set; }

        public string type { get; set; }
    }
}
