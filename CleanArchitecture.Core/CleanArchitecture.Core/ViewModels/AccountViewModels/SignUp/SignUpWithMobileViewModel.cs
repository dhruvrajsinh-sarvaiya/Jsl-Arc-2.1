using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
    public class SignUpWithMobileViewModel : TrackerViewModel
    {      
        [Required(ErrorMessage = "1,Please Enter Mobile Number, 4012")]
        [Phone(ErrorMessage = "1,Please Enter Valid Mobile Number, 4013")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "1,Please Enter Valid Mobile Number, 4014")]
        [Range(10, Int64.MaxValue)]
        public string Mobile { get; set; }
    }

    public class SignUpWithMobileResponse : BizResponseClass
    {

    }
}
