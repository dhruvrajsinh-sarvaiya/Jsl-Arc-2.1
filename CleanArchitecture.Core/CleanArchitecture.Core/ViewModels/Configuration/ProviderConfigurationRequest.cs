using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ProviderConfigurationRequest : ProviderConfigurationViewModel
    {
        
    }
    public class ProviderConfigurationResponce : BizResponseClass
    {
        public ProviderConfigurationViewModel response { get; set; }
    }

}
