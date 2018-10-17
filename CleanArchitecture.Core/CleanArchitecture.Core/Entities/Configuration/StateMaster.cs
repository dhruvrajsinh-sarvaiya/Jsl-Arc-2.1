using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class StateMaster : BizBase
    {
        [Required]
        [StringLength(30)]
        public string StateName { get; set; }
        [Required]
        [StringLength(2)]
        public string StateCode { get; set; }
        [Required]
        public string CountryID { get; set; }
    }
}
