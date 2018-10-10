using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceConfigurationResponse : BizResponseClass
    {
        public ServiceConfigurationInfo response { get; set; }
    }
    public class ServiceConfigurationInfo
    {
        public long ServiceId { get; set; }
    }

}
