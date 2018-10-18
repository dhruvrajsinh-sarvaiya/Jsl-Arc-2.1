using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class RouteConfigurationResponse : BizResponseClass
    {
        public RouteConfigurationInfo response { get; set; }
    }
    public class RouteConfigurationInfo
    {
        public long RouteId { get; set; }
    }
}