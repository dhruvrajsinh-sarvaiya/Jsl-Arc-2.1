using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class WebAPIParseResponse
    {        
        public decimal Balance { get; set; }
        public int Status { get; set; }
        public string StatusMsg { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public string ErrorCode { get; set; }
        public string TrnRefNo { get; set; }
        public string OperatorRefNo { get; set; }             
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
    }

    public class GetDataForParsingAPI
    {
        public string ResponseSuccess { get; set; }
        public string ResponseFailure { get; set; }
        public string ResponseHold { get; set; }
        public string BalanceRegex { get; set; }
        public string StatusRegex { get; set; }
        public string StatusMsgRegex { get; set; }
        public string ResponseCodeRegex { get; set; }
        public string ErrorCodeRegex { get; set; }        
        public string TrnRefNoRegex { get; set; }
        public string OprTrnRefNoRegex { get; set; }       
        public string Param1Regex { get; set; }
        public string Param2Regex { get; set; }
        public string Param3Regex { get; set; }
    }
}
