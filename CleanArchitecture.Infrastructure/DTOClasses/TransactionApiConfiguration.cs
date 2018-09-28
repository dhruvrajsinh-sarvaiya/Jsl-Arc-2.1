using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class TransactionApiConfigurationRequest
    {
        [Required]
        public string SMSCode { get; set; }
        enWebAPIRouteType APIType { get; set; }
    }
    public class TransactionProviderResponse
    {
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public long SerProID { get; set; }
        public string SerProName { get; set; }
        public long RouteID { get; set; }
        public string RouteName { get; set; }
        //public string SMSCode { get; set; }
        public short ServiceType { get; set; }
        public long ThirPartyAPIID { get; set; }      
        public short AppType { get; set; }
    }

    public class WebApiConfigurationResponse
    { 
        public long ThirPartyAPIID { get; set; }
        public string APISendURL { get; set; }
        public string APIValidateURL { get; set; }
        public string APIBalURL { get; set; }
        public string APIStatusCheckURL { get; set; }
        public string APIRequestBody { get; set; }
        public string TransactionIdPrefix { get; set; }
        public string MerchantCode { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string AuthHeader { get; set; }
        public string ContentType { get; set; }
        public string MethodType { get; set; }
        public string HashCode { get; set; }
        public string HashCodeRecheck { get; set; }
        public short HashType { get; set; }
        public short AppType { get; set; }
    }
}
