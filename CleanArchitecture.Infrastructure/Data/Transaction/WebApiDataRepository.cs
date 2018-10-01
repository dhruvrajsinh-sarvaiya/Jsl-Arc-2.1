using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class WebApiDataRepository
    {
        private readonly CleanArchitectureContext _dbContext;
        public WebApiDataRepository(CleanArchitectureContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public WebApiConfigurationResponse GetThirdPartyAPIData(long ThirPartyAPIID)
        {
            var result = from TP in _dbContext.ThirPartyAPIConfiguration
                                where TP.Id == ThirPartyAPIID && TP.Status == 1
                                select new WebApiConfigurationResponse
                                {
                                    ThirPartyAPIID = TP.Id,
                                    APISendURL = TP.APISendURL,
                                    APIValidateURL = TP.APIValidateURL,
                                    APIBalURL = TP.APIBalURL,
                                    APIStatusCheckURL = TP.APIStatusCheckURL,
                                    APIRequestBody = TP.APIRequestBody,
                                    TransactionIdPrefix = TP.TransactionIdPrefix,
                                    MerchantCode = TP.MerchantCode,
                                    UserID = TP.UserID,
                                    Password = TP.Password,
                                    AuthHeader = TP.AuthHeader,
                                    ContentType = TP.ContentType,
                                    MethodType = TP.MethodType,
                                    HashCode = TP.HashCode,
                                    HashCodeRecheck = TP.HashCodeRecheck,
                                    HashType = TP.HashType,
                                    AppType = TP.AppType
                                };
            return result.FirstOrDefault();
        }
        public GetDataForParsingAPI GetDataForParsingAPI(long ThirPartyAPIID)
        {
            var result = from TP in _dbContext.ThirPartyAPIConfiguration
                         join Regex in _dbContext.ThirPartyAPIResponseConfiguration on TP.ParsingDataID equals Regex.Id
                         where TP.Id == ThirPartyAPIID && TP.Status == 1
                         select new GetDataForParsingAPI
                         {
                             ResponseSuccess = TP.ResponseSuccess,
                             ResponseFailure = TP.ResponseFailure,
                             ResponseHold = TP.ResponseHold,
                             BalanceRegex = Regex.BalanceRegex,
                             StatusRegex = Regex.StatusRegex,
                             StatusMsgRegex = Regex.StatusMsgRegex,
                             ResponseCodeRegex =Regex.ResponseCodeRegex,
                             ErrorCodeRegex = Regex.ErrorCodeRegex,
                             TrnRefNoRegex = Regex.TrnRefNoRegex,
                             OprTrnRefNoRegex = Regex.OprTrnRefNoRegex,
                             Param1Regex = Regex.Param1Regex,
                             Param2Regex = Regex.Param2Regex,
                             Param3Regex = Regex.Param3Regex
                         };
            return result.FirstOrDefault();
        }
    }
}
