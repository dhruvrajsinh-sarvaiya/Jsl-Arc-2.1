using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.DTOClasses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class NewTransaction
    {
        BizResponse _Resp;
        readonly ILogger _log;
        BizResponse _CreateTransactionResp;

        public NewTransaction(ILogger log)
        {
            _log = log;
        }

        public BizResponse ProcessNewTransaction(NewTransactionRequestCls Req)
        {
            _Resp = new BizResponse();
           

            //=========================INSERT

            //Take memberMobile for sms



            //=========================PROCESS



            //=========================UPDATE



            return null;
        }

        public BizResponse InsertTransactionInQueue(NewTransactionRequestCls Req)
        {
            string TrnTypeName = "";
            try
            {
                return (new BizResponse { ReturnMsg = "", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);                
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
            }

        }

    }
}
