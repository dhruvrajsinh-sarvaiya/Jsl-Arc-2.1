﻿using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.OTP
{
    public class OTPWithMobileViewModel : TrackerViewModel
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [Range(6, Int64.MaxValue)]
        public string OTP { get; set; }

        [Required]
        [Phone]
        [StringLength(10, MinimumLength = 10)]
        [Range(10, Int64.MaxValue)]
        public string MobileNo { get; set; }
    }
}
