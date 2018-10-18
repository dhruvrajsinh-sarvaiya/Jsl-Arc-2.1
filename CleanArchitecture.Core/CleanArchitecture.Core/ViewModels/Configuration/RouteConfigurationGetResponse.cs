using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class RouteConfigurationGetResponse : BizResponseClass
    {
        public RouteConfigurationRequest response { get; set; }
    }
}
