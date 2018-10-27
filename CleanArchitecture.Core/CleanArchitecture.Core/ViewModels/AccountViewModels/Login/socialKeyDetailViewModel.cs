using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class SocialKeyDetailViewModel
    {
        public string ProviderName { get; set; }
        public string ClientId  { get; set; }
        public string ClientSecret { get; set; }
        
    }

    public class SocialKeyDetailResponse : BizResponseClass
    {
      public  SocialKeyDetailViewModel socialKeyDetailViewModel { get; set; }
    }
}
