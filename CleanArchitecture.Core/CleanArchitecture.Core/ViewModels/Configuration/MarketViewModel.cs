using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class MarketViewModel
    {
        public long ID { get; set; }
        [Required]
        public string CurrencyName { get; set; }
        public short isBaseCurrency { get; set; }
        [Required]
        public long ServiceID { get; set; }
    }

    public class MarketResponse : BizResponseClass
    {
        public List<MarketViewModel> response { get; set; }
    }
}
