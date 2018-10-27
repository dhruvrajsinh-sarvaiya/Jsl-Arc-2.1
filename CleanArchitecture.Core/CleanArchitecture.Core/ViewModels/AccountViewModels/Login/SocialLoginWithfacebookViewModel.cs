using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
  public  class SocialLoginWithfacebookViewModel : TrackerViewModel
    {

        //[Required(ErrorMessage = "1,Please Enter User Name,4001")]   /// Commanted by pankaj this field is not required for login with fb.(providerkey,name and token provide the detail of login fb detail so.)
             
        //public string FbUsername { get; set; }
        [Required(ErrorMessage = "2,Please Enter ProviderKey,4092")]
        public string ProviderKey { get; set; }
        [Required(ErrorMessage = "3,Please Enter ProviderName,4093")]
        public string ProviderName { get; set; }
        [Required(ErrorMessage = "4,Please Enter access_token,4094")]
        public string access_token { get; set; }
    }
    public class SocialLoginfacebookResponse : BizResponseClass
    {
        public  string Appkey { get; set; }
    }

}
