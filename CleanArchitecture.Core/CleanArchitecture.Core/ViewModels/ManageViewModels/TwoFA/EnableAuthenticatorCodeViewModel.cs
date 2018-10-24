using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels.TwoFA
{
   public  class EnableAuthenticatorCodeViewModel
    {

        [Required(ErrorMessage = "1,Please enter a valid twoFa Code,4082")]
        [Display(Name = "Verification Code")]
        [StringLength(6, ErrorMessage = "1,Please enter a valid twoFa Code,4082")]
        public string Code { get; set; }
    }
}
