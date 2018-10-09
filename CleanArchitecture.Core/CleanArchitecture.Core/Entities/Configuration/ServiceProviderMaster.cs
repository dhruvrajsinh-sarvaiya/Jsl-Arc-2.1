using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ServiceProviderMaster :BizBase
    {
        [Required]
        [StringLength(60)]
        public string ProviderName { get; set; }

        [Required]
        public long ServiceProID { get; set; }
    }
}
