using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
  public   class AddPhoneNumberViewModel
    {
        [Required(ErrorMessage = "1,Please Enter Mobile Number, 4012")]
        [Phone(ErrorMessage = "1,Please Enter Valid Mobile Number, 4013")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "1,Please Enter Valid Mobile Number, 4014")]
        [Range(10, Int64.MaxValue)]
     
        public string PhoneNumber { get; set; }
    }
}
