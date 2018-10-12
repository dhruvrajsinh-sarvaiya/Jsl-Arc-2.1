using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.OTP
{
    public class OTPWithEmailViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "0,Please enter a OTP number,400")]
        [StringLength(6, ErrorMessage = "0,The OTP must be between 6 digits,400", MinimumLength = 6)]        
        [Range(6, Int64.MaxValue)]
        public string OTP { get; set; }
        

        [Required(ErrorMessage = "0,Please enter a Email Address,4007")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid Email Id,4008")]
        [RegularExpression(@"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "0,Please enter a valid Email Address,4009")]
        public string Email { get; set; }
    }
    public class OTPWithEmailResponse:BizResponseClass
    {

    }
}
