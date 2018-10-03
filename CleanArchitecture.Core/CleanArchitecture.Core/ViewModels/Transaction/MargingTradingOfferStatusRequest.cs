using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MargingTradingOfferStatusRequest
    {
        [Required]
        public long offer_id { get; set; }
    }
}
