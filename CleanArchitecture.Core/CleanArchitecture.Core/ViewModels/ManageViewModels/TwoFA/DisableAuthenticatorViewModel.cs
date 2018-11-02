using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels.TwoFA
{
   public class DisableAuthenticatorViewModel
    {
        
        [Required(ErrorMessage = "1,Please enter veryfication code,4080")]
        [StringLength(7, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long.,4011,", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

       
    }

    public class DisableAuthenticatorResponse : BizResponseClass
    {
        //public DisableAuthenticatorResponse disableAuthenticatorViewModel { get; set; }
    }
}
