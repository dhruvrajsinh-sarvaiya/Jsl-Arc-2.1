using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class CountryMaster : BizBase
    {
        [Required]
        [StringLength(30)]
        public string CountryName { get; set; }
        [Required]
        [StringLength(2)]
        public string CountryCode { get; set; }
    }
}
