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
        public enWebAPIRouteType APIType { get; set; }
        public enTrnType trnType { get; set; } // ntrivedi  added 03-11-2018
        public decimal amount { get; set; }// ntrivedi  added 03-11-2018
    }
    public class TransactionProviderResponse
    {
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public long SerProID { get; set; }
        public string SerProName { get; set; }
        public long RouteID { get; set; }
        public long ProductID { get; set; } // ntrivedi added 03-11-2018
        public string RouteName { get; set; }
        //public string SMSCode { get; set; }
        public short ServiceType { get; set; }
        public long ThirPartyAPIID { get; set; }      
        public short AppType { get; set; }
        public short MinimumAmountItem { get; set; }
        public short MaximumAmountItem { get; set; }
        public short MinimumAmountService { get; set; }
        public short MaximumAmountService { get; set; }

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

    //ntrivedi moved from webapiparseresponsecls.cs
    public class GetDataForParsingAPI
    {
        public string ResponseSuccess { get; set; } = "";
        public string ResponseFailure { get; set; } = "";
        public string ResponseHold { get; set; } = "";
        public string BalanceRegex { get; set; } = "";
        public string StatusRegex { get; set; } = "";
        public string StatusMsgRegex { get; set; } = "";
        public string ResponseCodeRegex { get; set; } = "";
        public string ErrorCodeRegex { get; set; } = "";
        public string TrnRefNoRegex { get; set; } = "";
        public string OprTrnRefNoRegex { get; set; } = "";
        public string Param1Regex { get; set; } = "";
        public string Param2Regex { get; set; } = "";
        public string Param3Regex { get; set; } = "";
    }
}
