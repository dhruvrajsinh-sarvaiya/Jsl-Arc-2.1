using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class CityMaster : BizBase
    {
        [Required]
        [StringLength(30)]
        public string CityName { get; set; }
        [Required]
        public long StateID { get; set; }

    }
}
