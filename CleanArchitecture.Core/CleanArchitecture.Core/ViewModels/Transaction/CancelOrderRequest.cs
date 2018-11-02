using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class CancelOrderRequest
    {
        [Required]
        public long TranNo { get; set; }
    }
}
