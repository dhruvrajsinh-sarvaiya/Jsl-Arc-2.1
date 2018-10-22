using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ZipCodeMaster : BizBase
    {
        [Required]
        public long ZipCode { get; set; }
        [Required]
        [StringLength(30)]
        public string ZipAreaName { get; set; }
        [Required]
        public long CityID { get; set; }
    }
}
