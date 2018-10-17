using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class LoginWithMobileViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,Please Enter Mobile Number, 4012")]
        [Phone(ErrorMessage = "1,Please Enter Valid Mobile Number, 4013")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "1,Please Enter Valid Mobile Number, 4014")]
        [Range(10, Int64.MaxValue)]
        public string Mobile { get; set; }

        //public bool RememberMe { get; set; }
    }

    public class LoginWithMobileResponse : BizResponseClass
    {
        public string appkey { get; set; }
    }
}
