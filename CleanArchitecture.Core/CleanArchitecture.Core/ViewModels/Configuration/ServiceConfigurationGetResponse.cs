using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceConfigurationGetResponse : BizResponseClass
    {
        public ServiceConfigurationRequest response { get; set; }
    }

}
