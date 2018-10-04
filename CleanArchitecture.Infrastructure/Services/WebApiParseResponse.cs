using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data.Transaction;
using CleanArchitecture.Infrastructure.DTOClasses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Infrastructure.Services
{
    //Common Parsing method Implement Here
    public class WebApiParseResponse : IWebApiParseResponse<WebAPIParseResponseCls>
    {
        readonly ILogger _log;
        readonly TransactionWebAPIConfiguration _txnWebAPIConf;
        GetDataForParsingAPI _txnWebAPIParsingData;
        WebAPIParseResponseCls _webapiParseResponse;
        private readonly WebApiDataRepository _webapiDataRepository;
        public WebApiParseResponse(ILogger log, TransactionWebAPIConfiguration txnWebAPIConf, WebApiDataRepository webapiDataRepository)
        {
            _log = log;
            _txnWebAPIConf = txnWebAPIConf;
            _webapiDataRepository = webapiDataRepository;
        }
        public WebAPIParseResponseCls TransactionParseResponse(string TransactionResponse, long ThirPartyAPIID)
        {
            try
            {
                //Take Regex for response parsing
                _txnWebAPIParsingData = _webapiDataRepository.GetDataForParsingAPI(ThirPartyAPIID);

                _webapiParseResponse = ParseResponseViaRegex(TransactionResponse, _txnWebAPIParsingData);

                return _webapiParseResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return null;
            }
        }
        public string CheckArrayLengthAndReturnResponse(string StrResponse, string[] strArray)
        {
            string ResponseFromParsing = "";
            try
            {
                if (strArray != null && strArray.Length > 1)
                {
                    ResponseFromParsing = ParseResponse(StrResponse, strArray[0], strArray[1]);
                }//either Send blank                
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
            }
            return ResponseFromParsing;
        }
        public string ParseResponse(string StrResponse, string regex1, string regex2)
        {
            string MatchRegex = "";
            string MatchRegex2 = "";
            try
            {
                if (regex1 != null && regex2 != null)
                {
                    MatchRegex = Regex.Match(StrResponse.ToLower(), regex1.ToLower(), new RegexOptions()).Value;
                    if ((!string.IsNullOrEmpty(MatchRegex)))
                    {
                        MatchRegex2 = Regex.Replace(MatchRegex, regex2.ToLower(), "");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "::" + regex1 + "::" + regex2 + "\nClassname=" + this.GetType().Name, LogLevel.Error);
            }
            return MatchRegex2;
        }
        public WebAPIParseResponseCls ParseResponseViaRegex(string Response, GetDataForParsingAPI _txnWebAPIParsingData)
        {
            try
            {
                string[] BalanceArray = _txnWebAPIParsingData.BalanceRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StatusArray = _txnWebAPIParsingData.StatusRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StatusMsgArray = _txnWebAPIParsingData.StatusMsgRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] ResponseCodeArray = _txnWebAPIParsingData.ResponseCodeRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] ErrorCodeArray = _txnWebAPIParsingData.ErrorCodeRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] TrnRefNoArray = _txnWebAPIParsingData.TrnRefNoRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] OprTrnRefNoArray = _txnWebAPIParsingData.OprTrnRefNoRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param1Array = _txnWebAPIParsingData.Param1Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param2Array = _txnWebAPIParsingData.Param2Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param3Array = _txnWebAPIParsingData.Param3Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string BalanceResp = CheckArrayLengthAndReturnResponse(Response, BalanceArray);
                string StatusResp = CheckArrayLengthAndReturnResponse(Response, StatusArray);
                string StatusMsgResp = CheckArrayLengthAndReturnResponse(Response, StatusMsgArray);
                string TrnRefNoResp = CheckArrayLengthAndReturnResponse(Response, TrnRefNoArray);
                string OprTrnRefNoResp = CheckArrayLengthAndReturnResponse(Response, OprTrnRefNoArray);
                string ResponseCodeResp = CheckArrayLengthAndReturnResponse(Response, ResponseCodeArray);
                string ErrorCodeResp = CheckArrayLengthAndReturnResponse(Response, ErrorCodeArray);
                string Param1Resp = CheckArrayLengthAndReturnResponse(Response, Param1Array);
                string Param2Resp = CheckArrayLengthAndReturnResponse(Response, Param2Array);
                string Param3Resp = CheckArrayLengthAndReturnResponse(Response, Param3Array);


                if (_txnWebAPIParsingData.ResponseSuccess.Contains(StatusResp))
                {
                    _webapiParseResponse.Status = enTransactionStatus.Success;
                }
                else if (_txnWebAPIParsingData.ResponseFailure.Contains(StatusResp))
                {
                    _webapiParseResponse.Status = enTransactionStatus.OperatorFail;
                }
                else
                {
                    _webapiParseResponse.Status = enTransactionStatus.Hold;
                }
                if (!string.IsNullOrEmpty(BalanceResp))
                    _webapiParseResponse.Balance = Convert.ToDecimal(BalanceResp);

                _webapiParseResponse.StatusMsg = StatusResp;
                _webapiParseResponse.ResponseMsg = StatusMsgResp;
                _webapiParseResponse.ResponseCode = ResponseCodeResp;
                _webapiParseResponse.ErrorCode = ErrorCodeResp;
                _webapiParseResponse.TrnRefNo = TrnRefNoResp;
                _webapiParseResponse.OperatorRefNo = OprTrnRefNoResp;
                _webapiParseResponse.Param1 = Param1Resp;
                _webapiParseResponse.Param2 = Param2Resp;
                _webapiParseResponse.Param3 = Param3Resp;

                return _webapiParseResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return null;
            }
        }
    }
}

