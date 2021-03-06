﻿using CleanArchitecture.Core.Enums;
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
        public int trnType { get; set; } // ntrivedi  added 03-11-2018
        public decimal amount { get; set; }// ntrivedi  added 03-11-2018
    }
    public class TransactionProviderResponse
    {
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public long ServiceProID { get; set; }
        public long SerProDetailID { get; set; }
        //public string SerProName { get; set; } Rushabh 11-10-2018 Removed because there is no column in database
        public long RouteID { get; set; }
        public long ProductID { get; set; } // ntrivedi added 03-11-2018
        public string RouteName { get; set; }
        //public string SMSCode { get; set; }
        public short ServiceType { get; set; }
        public long ThirPartyAPIID { get; set; }      
        public long AppTypeID { get; set; } //Rushabh Updated 11-10-2018 oldName=AppType
        public decimal MinimumAmountItem { get; set; } //Rushabh Updated 11-10-2018 old datatype=long
        public decimal MaximumAmountItem { get; set; } //Rushabh Updated 11-10-2018 old datatype=long
        //Rushabh Updated 15-10-2018 As query doesn't return these parameters anymore
        //public decimal MinimumAmountService { get; set; } //Rushabh Updated 11-10-2018 old datatype=long
        //public decimal MaximumAmountService { get; set; } //Rushabh Updated 11-10-2018 old datatype=long

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
    public class TradeHistoryResponce
    {
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public short Status { get; set; }
        public string StatusText { get; set; }
        public String PairName { get; set; }
        public Decimal ChargeRs { get; set; }
        public short IsCancelled { get; set; }
        public short ordertype { get; set; }
    }
    public class RecentOrderRespose 
    {
        public short ordertype { get; set; }
        public long TrnNo { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Qty { get; set; }
        public DateTime DateTime { get; set; }
        public short Status { get; set; }
        public string PairName { get; set; }
        public long PairId { get; set; }
    }
    public class GetGraphResponse
    {
        public DateTime DataDate { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal OpenVal { get; set; }
        public decimal CloseVal { get; set; }
        public decimal Volume { get; set; }
    }
    public class GetTradingSummary
    {
        public long TrnNo { get; set; }
        public Int32 MemberID { get; set; }
        public string Type { get; set; }
        public Decimal Price { get; set; }
        public Decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string StatusText { get; set; }
        public long PairID { get; set; }
        public String PairName { get; set; }
        public Decimal ChargeRs { get; set; }
        public Decimal PreBal { get; set; }
        public Decimal PostBal { get; set; }
    }

    public class ServiceMasterResponse
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string SMSCode { get; set; }
        public short ServiceType { get; set; }
        public string ServiceDetailJson { get; set; }
        public long CirculatingSupply { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal IssuePrice { get; set; }
        public short TransactionBit { get; set; }
        public short WithdrawBit { get; set; }
        public short DepositBit { get; set; }

    }
    public class GetGraphResponsePairWise
    {
        public DateTime DataDate { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal OpenVal { get; set; }
        public decimal CloseVal { get; set; }
        public decimal Volume { get; set; }
        public string PairName { get; set; }
    }
}
