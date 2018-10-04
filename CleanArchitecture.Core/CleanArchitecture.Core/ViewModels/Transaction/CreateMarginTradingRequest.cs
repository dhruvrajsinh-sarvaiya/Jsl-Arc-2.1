using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateMarginTradingRequest
    {
        [Required]
        public string currency { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal amount { get; set; }

        [Required]
        public decimal rate { get; set; }

        [Required]
        public int period { get; set; }

        [Required]
        public string direction { get; set; }
    }
}
