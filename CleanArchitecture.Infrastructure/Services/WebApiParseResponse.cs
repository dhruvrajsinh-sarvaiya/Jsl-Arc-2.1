using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data.Transaction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Infrastructure.Services
{
    //Common Parsing method Implement Here
    class WebApiParseResponse : IWebApiParseResponse<WebAPIParseResponse>
    {
        readonly ILogger<WalletService> _log;
        readonly TransactionWebAPIConfiguration _txnWebAPIConf;
        GetDataForParsingAPI _txnWebAPIParsingData;
        WebAPIParseResponse _webapiParseResponse;
        private readonly WebApiDataRepository _webapiDataRepository;
        public WebApiParseResponse(ILogger<WalletService> log, TransactionWebAPIConfiguration txnWebAPIConf, WebApiDataRepository webapiDataRepository)
        {
            _log = log;
            _txnWebAPIConf = txnWebAPIConf;
            _webapiDataRepository = webapiDataRepository;
        }
        public WebAPIParseResponse TransactionParseResponse(string TransactionResponse, long ThirPartyAPIID)
        {
            try
            {
                //Take Regex for response parsing
                _txnWebAPIParsingData = _webapiDataRepository.GetDataForParsingAPI(ThirPartyAPIID);

                //in table save two regex in one column , 1st-Match around String ,2nd- replace not used string in getting string in 1st regex
                string[] BalanceArray = _txnWebAPIParsingData.StatusRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StatusArray = _txnWebAPIParsingData.StatusRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StatusMsgArray = _txnWebAPIParsingData.StatusMsgRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] ResponseCodeArray = _txnWebAPIParsingData.StatusMsgRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] ErrorCodeArray = _txnWebAPIParsingData.StatusMsgRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] TrnRefNoArray = _txnWebAPIParsingData.TrnRefNoRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] OprTrnRefNoArray = _txnWebAPIParsingData.OprTrnRefNoRegex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param1Array = _txnWebAPIParsingData.Param1Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param2Array = _txnWebAPIParsingData.Param2Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] Param3Array = _txnWebAPIParsingData.Param3Regex.Split("###".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string BalanceResp = CheckArrayLengthAndReturnResponse(TransactionResponse, BalanceArray);
                string StatusResp = CheckArrayLengthAndReturnResponse(TransactionResponse, StatusArray);
                string StatusMsgResp = CheckArrayLengthAndReturnResponse(TransactionResponse, StatusMsgArray);
                string TrnRefNoResp = CheckArrayLengthAndReturnResponse(TransactionResponse, TrnRefNoArray);
                string OprTrnRefNoResp = CheckArrayLengthAndReturnResponse(TransactionResponse, OprTrnRefNoArray);
                string ResponseCodeResp = CheckArrayLengthAndReturnResponse(TransactionResponse, ResponseCodeArray);
                string ErrorCodeResp = CheckArrayLengthAndReturnResponse(TransactionResponse, ErrorCodeArray);
                string Param1Resp = CheckArrayLengthAndReturnResponse(TransactionResponse, Param1Array);
                string Param2Resp = CheckArrayLengthAndReturnResponse(TransactionResponse, Param2Array);
                string Param3Resp = CheckArrayLengthAndReturnResponse(TransactionResponse, Param3Array);


                if (_txnWebAPIParsingData.ResponseSuccess.Contains(StatusResp))
                {
                    _webapiParseResponse.Status = Convert.ToInt16(enTransactionStatus.Success);
                }
                else if (_txnWebAPIParsingData.ResponseFailure.Contains(StatusResp))
                {
                    _webapiParseResponse.Status = Convert.ToInt16(enTransactionStatus.OperatorFail);
                }
                else
                {
                    _webapiParseResponse.Status = Convert.ToInt16(enTransactionStatus.Hold);
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
    }
}

