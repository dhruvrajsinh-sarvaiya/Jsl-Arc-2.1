using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ThirdPartyAPIConfigViewModel
    {
        public long Id { get; set; }
        public string APIName { get; set; }
        public string APISendURL { get; set; }
        public string APIValidateURL { get; set; }
        public string APIBalURL { get; set; }
        public string APIStatusCheckURL { get; set; }
        public string APIRequestBody { get; set; }
        public string TransactionIdPrefix { get; set; }
        public string MerchantCode { get; set; }
        //public string UserID { get; set; }
        //public string Password { get; set; }
        public long SerProConfigurationID { get; set; }
        public string ResponseSuccess { get; set; }
        public string ResponseFailure { get; set; }
        public string ResponseHold { get; set; }
        public string AuthHeader { get; set; }
        public string ContentType { get; set; }
        public string MethodType { get; set; }
        public string HashCode { get; set; }
        public string HashCodeRecheck { get; set; }
        public short HashType { get; set; }
        public short AppType { get; set; }
        public long ParsingDataID { get; set; }
    }
}
