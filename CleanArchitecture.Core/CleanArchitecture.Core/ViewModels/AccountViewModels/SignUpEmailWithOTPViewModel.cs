using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SignUpEmailWithOTPViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
