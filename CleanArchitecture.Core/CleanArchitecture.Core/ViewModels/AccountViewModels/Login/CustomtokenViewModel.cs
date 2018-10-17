using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class CustomtokenViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Password { get; set; }
        public bool EnableStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class Customtokenresponse : BizResponseClass
    {
        //public object SignIntoken { get; set; }
    }

}
