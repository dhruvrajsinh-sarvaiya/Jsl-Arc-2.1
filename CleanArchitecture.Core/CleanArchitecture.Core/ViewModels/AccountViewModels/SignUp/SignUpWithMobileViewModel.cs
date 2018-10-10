using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
    public class SignUpWithMobileViewModel : TrackerViewModel
    {
        [Required]        
        [StringLength(10, MinimumLength = 10)]
        [Range(10, Int64.MaxValue)]
        public string Mobile { get; set; }
    }

    public class SignUpWithMobileResponse : BizResponseClass
    {

    }
}
