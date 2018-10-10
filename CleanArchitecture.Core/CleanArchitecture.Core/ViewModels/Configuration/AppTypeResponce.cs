using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class AppTypeResponce : BizResponseClass
    {
        public IEnumerable <AppTypeViewModel> responce { get; set; }
    }
    public class AppTypeResponceData : BizResponseClass
    {
        public AppTypeViewModel responce { get; set; }
    }
}
