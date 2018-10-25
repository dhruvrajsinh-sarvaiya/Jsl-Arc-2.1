using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class MarketViewModel
    {
        [Required]
        public string CurrencyName { get; set; }
        public short isBaseCurrency { get; set; }
        public long ServiceID { get; set; }
    }
}
