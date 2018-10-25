using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    class SocialLoginWithfacebookViewModel : TrackerViewModel
    {

        [Required(ErrorMessage = "1,Please Enter User Name,4001")]
        
      
        public string FbUsername { get; set; }
        [Required(ErrorMessage = "1,Please Enter ProviderKey,4092")]
        public string ProviderKey { get; set; }
        [Required(ErrorMessage = "1,Please Enter ProviderName,4092")]
        public string ProviderName { get; set; }
        [Required(ErrorMessage = "1,Please Enter access_token,4094")]
        public string access_token { get; set; }
    }
}
