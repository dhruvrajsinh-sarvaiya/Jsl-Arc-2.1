using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class TransactionApiConfigurationRequest
    {
        [Required]
        public long TrnNo { get; set; }       
    }
    public class TransactionApiConfigurationResponse
    {
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public long SerProID { get; set; }
        public string SerProName { get; set; }
        public long RouteID { get; set; }
        public string RouteName { get; set; }
        public string SMSCode { get; set; }
        public short ServiceType { get; set; }
        public long ThirdPartyAPIServiceId { get; set; }
    }
}
