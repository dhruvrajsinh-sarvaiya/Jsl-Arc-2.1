using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class MarginTradingClosePositionRequest
    {
        [Required]
        public long position_id { get; set; }
    }
}
