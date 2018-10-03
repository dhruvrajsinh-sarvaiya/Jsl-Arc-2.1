using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CreateOrderRequest
    {
        [Required]
        public string pair_name { get; set; }

        [Required]
        public string side { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal price { get; set; }

        [Required]
        public string ordertype { get; set; }

        [Required]
        [Range(0, 9999999999.99999999)]
        public decimal volume { get; set; }

    }
}
