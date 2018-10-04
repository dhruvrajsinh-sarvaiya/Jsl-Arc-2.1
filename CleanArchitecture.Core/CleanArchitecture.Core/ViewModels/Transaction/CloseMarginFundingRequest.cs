using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CloseMarginFundingRequest
    {
        [Required]
        public Int32 swap_id { get; set; }
    }
}
