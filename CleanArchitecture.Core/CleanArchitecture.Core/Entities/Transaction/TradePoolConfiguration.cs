using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Transaction
{
    public class TradePoolConfiguration : BizBase
    {
        [Required]
        public long CountPerPrice { get; set; }
    }
}
