using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SignUpMobileWithOTPViewModel
    {
        [Required]
        [Phone]
        public string Mobile { get; set; }
    }
}
