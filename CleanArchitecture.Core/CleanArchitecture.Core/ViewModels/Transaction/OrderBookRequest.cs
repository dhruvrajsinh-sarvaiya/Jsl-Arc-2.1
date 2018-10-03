using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class OrderBookRequest
    {
        [Required]
        public string Pair { get; set; }

        public Int16 Count { get; set; }
    }
    public class OrderBookResponce
    {
        public int lastUpdateId { get; set; }
        public List<List<object>> bids { get; set; }
        public List<List<object>> asks { get; set; }
    }
}
