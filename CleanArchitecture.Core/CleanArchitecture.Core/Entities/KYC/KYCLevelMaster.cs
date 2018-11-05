using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.KYC
{
    public class KYCLevelMaster : BizBase
    {
        [Required]
        [StringLength(150)]
        public string KYCName { get; set; }

        public int Level { get; set; }

        public bool EnableStatus { get; set; }

        public bool IsDelete { get; set; }
    }
}
