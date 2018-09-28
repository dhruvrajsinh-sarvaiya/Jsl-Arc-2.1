﻿using CleanArchitecture.Core.ApiModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SignUpWithMobileViewModel : TrackerViewModel
    {
        [Required]        
        [StringLength(10, MinimumLength = 10)]
        [Range(10, Int64.MaxValue)]
        public string Mobile { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }
    }

    public class SignUpMobileWithOTPResponse : BizResponseClass
    {

    }
}
