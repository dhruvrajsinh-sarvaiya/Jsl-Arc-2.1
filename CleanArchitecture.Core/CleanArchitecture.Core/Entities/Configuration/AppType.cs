using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class AppType :BizBase
    {
        [Required]
        [StringLength(20)]
        public String AppTypeName { get; set; }
    }
}
