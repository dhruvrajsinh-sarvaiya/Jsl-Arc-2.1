using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.DTOClasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanArchitecture.Infrastructure.Data.Transaction
{
    public class WebApiDataRepository : IWebApiRepository
    {
        private readonly CleanArchitectureContext _dbContext;
        public readonly ILogger<WebApiDataRepository> _log;


        public WebApiDataRepository(CleanArchitectureContext dbContext, ILogger<WebApiDataRepository> log)
        {
            _dbContext = dbContext;
            _log = log;
        }

        public WebApiConfigurationResponse GetThirdPartyAPIData(long ThirPartyAPIID)
        {
            var result = from TP in _dbContext.ThirdPartyAPIConfiguration
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
                             //UserID = TP.UserID,
                             //Password = TP.Password,
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
            var result = from TP in _dbContext.ThirdPartyAPIConfiguration
                         join Regex in _dbContext.ThirdPartyAPIResponseConfiguration on TP.ParsingDataID equals Regex.Id
                         where TP.Id == ThirPartyAPIID && TP.Status == 1
                         select new GetDataForParsingAPI
                         {
                             ResponseSuccess = TP.ResponseSuccess,
                             ResponseFailure = TP.ResponseFailure,
                             ResponseHold = TP.ResponseHold,
                             BalanceRegex = Regex.BalanceRegex,
                             StatusRegex = Regex.StatusRegex,
                             StatusMsgRegex = Regex.StatusMsgRegex,
                             ResponseCodeRegex = Regex.ResponseCodeRegex,
                             ErrorCodeRegex = Regex.ErrorCodeRegex,
                             TrnRefNoRegex = Regex.TrnRefNoRegex,
                             OprTrnRefNoRegex = Regex.OprTrnRefNoRegex,
                             Param1Regex = Regex.Param1Regex,
                             Param2Regex = Regex.Param2Regex,
                             Param3Regex = Regex.Param3Regex
                         };
            return result.FirstOrDefault();
        }

        //ntrivedi fetch route
        public List<TransactionProviderResponse> GetProviderDataList(TransactionApiConfigurationRequest Request)
        {
            try
            {
                //and {2} between RC.MinimumAmount and RC.MaximumAmount
                //and {2}  between SC.MinimumAmount and SC.MaximumAmount
                //IQueryable<TransactionProviderResponse> Result = _dbContext.TransactionProviderResponse.FromSql(
                //            @"select SC.ID as ServiceID,SC.ServiceName,Prc.ID as SerProDetailID,Prc.ServiceProID,RC.ID as RouteID,
                //            PC.ID as ProductID,RC.RouteName,SC.ServiceType,Prc.ThirPartyAPIID,Prc.AppTypeID,RC.MinimumAmount as MinimumAmountItem,
                //            RC.MaximumAmount as MaximumAmountItem,SC.MinimumAmount as MinimumAmountService,SC.MaximumAmount as MaximumAmountService
                //            from ServiceConfiguration SC inner join  ProductConfiguration PC on
                //   PC.ServiceID = SC.Id inner join RouteConfiguration RC on RC.ProductID = PC.Id  
                //   inner join ServiceProviderDetail PrC on Prc.ServiceProID = RC.SerProID AND Prc.TrnTypeID={1} 
                //   where SC.SMSCode = {0} and RC.TrnType={1} 
                //   and {2} between RC.MinimumAmount and RC.MaximumAmount
                //   and {3} between SC.MinimumAmount and SC.MaximumAmount
                //   and SC.Status = 1 and RC.Status = 1 and Prc.Status=1 
                //   order by RC.Priority", Request.SMSCode, Request.trnType, Request.amount,Request.amount);

                // ntrivedi limit changes done 12-10-2018
                //Rita 13-10-2018  remove as no present in New table Service master
                //,SC.MinimumAmount as MinimumAmountService,SC.MaximumAmount as MaximumAmountService
                //and {3} between SC.MinimumAmount and SC.MaximumAmount
                //Rita 17-10-18 further make inner joi in RoutMaster with SerProDetailID with providerdetail
                IQueryable<TransactionProviderResponse> Result = _dbContext.TransactionProviderResponse.FromSql(
                           @"select SC.ID as ServiceID,SC.Name as ServiceName,Prc.ID as SerProDetailID,Prc.ServiceProID,RC.ID as RouteID,
                            PC.ID as ProductID,RC.RouteName,SC.ServiceType,Prc.ThirPartyAPIID,Prc.AppTypeID,LC.MinAmt as MinimumAmountItem,
                            LC.MaxAmt as MaximumAmountItem
                            from ServiceMaster SC inner join  ProductConfiguration PC on
			                PC.ServiceID = SC.Id inner join RouteConfiguration RC on RC.ProductID = PC.Id  
			                inner join ServiceProviderDetail PrC on Prc.Id = RC.SerProDetailID AND Prc.TrnTypeID={1} 
							inner join Limits LC on LC.ID = RC.LimitID 
			                where SC.SMSCode = {0}  and RC.TrnType={1} 
			                and {2} between LC.MinAmt and LC.MaxAmt			                
			                and SC.Status = 1 and RC.Status = 1 and Prc.Status=1 
			                order by RC.Priority", Request.SMSCode, Request.trnType, Request.amount, Request.amount);


                //return Result.ToList();Prc.SerProName,


                //IQueryable<TransactionProviderResponse> Result = _dbContext.TransactionProviderResponse.FromSql(
                //        @"select SC.ID as ServiceID,SC.ServiceName,Prc.ID as SerProID,Prc.SerProName,RC.ID as RouteID,PC.ID as ProductID,RC.RouteName,SC.ServiceType,
                //        Prc.ThirPartyAPIID,Prc.AppType,RC.MinimumAmount as MinimumAmountItem,RC.MaximumAmount as MaximumAmountItem,SC.MinimumAmount as MinimumAmountService,SC.MaximumAmount as MaximumAmountService
                //        from ServiceConfiguration SC inner
                //        join ProductConfiguration PC on
                //        PC.ServiceID = SC.Id inner
                //        join RouteConfiguration RC on RC.ProductID = PC.Id  inner
                //        join ProviderConfiguration PrC on Prc.Id = RC.SerProID
                //        where SC.SMSCode = {0} and RC.TrnType ={1}   and {2}
                //        between RC.MinimumAmount and RC.MaximumAmount and {3}
                //        between SC.MinimumAmount and SC.MaximumAmount
                //        and SC.Status = 1 and RC.Status = 1 and Prc.Status = 1
                //        order by RC.Priority ", Request.SMSCode,Request.trnType, Request.amount,Request.amount);



                var list = new List<TransactionProviderResponse>(Result);
                return list;
                //return Result.ToList();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "MethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
