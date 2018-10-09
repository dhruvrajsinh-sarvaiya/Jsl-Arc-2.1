using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ServiceProviderDetail : BizBase
    {
        [Required]
        public long ServiceProID { get; set; }

        [Required]
        public long AppTypeID { get; set; }

        [Required]
        public long TrnID { get; set; }

        [Required]
        public long LimitID { get; set; }
        
        public long DemonConfigID { get; set; }

        [Required]
        public long ServiceProConfigID { get; set; }
    }
}
