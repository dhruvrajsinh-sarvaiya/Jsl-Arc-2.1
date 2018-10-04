using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.DTOClasses;
using System.Linq;

namespace CleanArchitecture.Infrastructure.Data
{
    public class AddTransactionRepository //: ITransactionRepository<NewTransactionRequestCls, BizResponse>
    {
        readonly ILogger _log;
        BizResponse _Resp;
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;

        public AddTransactionRepository(EFCommonRepository<TransactionQueue> TransactionRepository, ILogger log)
        {
            _TransactionRepository = TransactionRepository;
            _log = log;
        }

        public BizResponse CreateTransaction(NewTransactionRequestCls NewtransactionReq)
        {
            _Resp = new BizResponse();
            try
            {
                var Newtransaction = new TransactionQueue()
                {                  
                    TrnMode = NewtransactionReq.TrnMode,
                    TrnType = Convert.ToInt16(NewtransactionReq.TrnType),
                    MemberID = NewtransactionReq.MemberID,
                    MemberMobile = NewtransactionReq.MemberMobile,                   
                    TransactionAccount = NewtransactionReq.TransactionAccount,
                    SMSCode = NewtransactionReq.SMSCode,
                    Amount = NewtransactionReq.Amount,                    
                    Status = 0,
                    StatusCode = 0,
                    StatusMsg = "Initialise",                   
                    TrnRefNo = NewtransactionReq.TrnRefNo,                   
                    AdditionalInfo = NewtransactionReq.AdditionalInfo
                };
                _TransactionRepository.Add(Newtransaction);
                return (new BizResponse { ReturnMsg = "Success", ReturnCode = enResponseCodeService.Success });
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);               
                return (new BizResponse { ReturnMsg = ex.Message, ReturnCode = enResponseCodeService.InternalError });
            }               
           
        }        
    }
}
