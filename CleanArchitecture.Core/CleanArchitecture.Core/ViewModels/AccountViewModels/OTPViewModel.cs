using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class OTPViewModel : TrackerViewModel
    {
        [Required]
        [Range(6, Int64.MaxValue)]
        public int OTP { get; set; }
    }
}
