using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.KYC
{
    public class KYCLevelViewModel : TrackerViewModel
    {
        [StringLength(150)]
        [Required]
        public string KYCName { get; set; }

        public int Level { get; set; }

        public bool EnableStatus { get; set; }

        public bool IsDelete { get; set; }
    }
}
