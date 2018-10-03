using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class OrderBookRequest
    {
        [Required]
        [StringLength(7, MinimumLength = 6)]
        public string Pair { get; set; }

        public long Count { get; set; }
    }
    public class OrderBookResponce
    {
        public long lastUpdateId { get; set; }
        public List<List<object>> bids { get; set; }
        public List<List<object>> asks { get; set; }
    }
}
