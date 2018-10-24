using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class UnLockUserViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,Please Enter User Id,4072")]
        //[StringLength(50, ErrorMessage = "1,Please Enter Valid User Id,4073")]
        public long UserId { get; set; }
    }

    public class UnLockUserResponseViewModel : BizResponseClass
    {
    }
}
