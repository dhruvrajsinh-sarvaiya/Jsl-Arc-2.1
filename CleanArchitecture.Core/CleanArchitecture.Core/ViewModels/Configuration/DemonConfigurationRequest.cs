using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class DemonConfigurationRequest : DemonconfigurationViewModel 
    {
       
    }
    public class DemonConfigurationResponce : BizResponseClass
    {
        public DemonconfigurationViewModel responce { get; set; }
    }
}
