using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class TradePairAssetRequest
    {
        [StringLength(7,MinimumLength=6)]
        public string Pair { get; set; }
    }
}
