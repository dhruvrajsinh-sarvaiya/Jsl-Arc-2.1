using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ServiceConfigurationGetAllResponse : BizResponseClass
    {
        public List<ServiceConfigurationRequest> response { get; set; }
    }
    public class GetServiceByBaseReasponse : BizResponseClass
    {
        public List<ServiceCurrencyData> response { get; set; }
    }
    public class ServiceCurrencyData
    {
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public string SMSCode { get; set; }
    }
}