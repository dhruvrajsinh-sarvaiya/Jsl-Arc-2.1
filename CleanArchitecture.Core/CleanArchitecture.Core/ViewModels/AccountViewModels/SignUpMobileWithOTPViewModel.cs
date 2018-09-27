﻿using CleanArchitecture.Core.ApiModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SignUpMobileWithOTPViewModel : TrackerViewModel
    {
        [Required]
        [Phone]
        public string Mobile { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class SignUpMobileWithOTPResponse : BizResponseClass
    {

    }
}
