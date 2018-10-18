using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class RouteConfigurationGetAllResponse : BizResponseClass
    {
        public List<RouteConfigurationRequest> response { get; set; }
    }
}
