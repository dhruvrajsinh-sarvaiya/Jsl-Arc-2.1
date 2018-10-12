using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.OTP
{
    public class OTPWithMobileViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,Please enter a OTP number,400")]
        [StringLength(6, ErrorMessage = "1,The OTP must be between 6 digits,400", MinimumLength = 6)]
        [Range(6, Int64.MaxValue)]
        public string OTP { get; set; }

        [Required(ErrorMessage = "1,Please Enter Mobile Number, 4012")]
        [Phone(ErrorMessage = "1,Please Enter Valid Mobile Number, 4013")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "1,Please Enter Valid Mobile Number, 4014")]
        [Range(10, Int64.MaxValue)]
        public string Mobile { get; set; }
    }

    public class OTPWithMobileResponse : BizResponseClass
    {

    }
}
