using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
   public  class FacebookAppAccessTokenViewModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
    }
}
