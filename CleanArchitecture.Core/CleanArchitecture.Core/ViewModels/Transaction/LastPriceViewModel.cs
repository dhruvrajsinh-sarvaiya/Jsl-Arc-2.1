using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class LastPriceViewModel
    {
        //for socket method
        [Required]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        public decimal LTP { get; set; }
        //public short UpDownBit { get; set; }
    }
}
