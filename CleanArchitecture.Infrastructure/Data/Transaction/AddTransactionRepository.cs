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

namespace CleanArchitecture.Infrastructure.Data
{
    public class AddTransactionRepository
    {
        readonly ILogger _log;
        BizResponseClass _Resp;
        private readonly IRepository<TransactionQueue> _TransactionRepository;

        public AddTransactionRepository(IRepository<TransactionQueue> TransactionRepository, ILogger log)
        {
            _TransactionRepository = TransactionRepository;
            _log = log;
        }

        public BizResponseClass CreateTransaction(TransactionQueue NewtransactionReq)
        {
            _Resp = new BizResponseClass();
            try
            {
                var Newtransaction = new TransactionQueue()
                {                  
                    TrnMode = NewtransactionReq.TrnMode,
                    TrnType = NewtransactionReq.TrnType,
                    MemberID = NewtransactionReq.MemberID,
                    MemberMobile = NewtransactionReq.MemberMobile,                   
                    TransactionAccount = NewtransactionReq.TransactionAccount,
                    SMSCode = NewtransactionReq.SMSCode,
                    Amount = NewtransactionReq.Amount,                    
                    Status = NewtransactionReq.Status,
                    StatusCode = NewtransactionReq.StatusCode,
                    StatusMsg = NewtransactionReq.StatusMsg,                   
                    TrnRefNo = NewtransactionReq.TrnRefNo                   
                };
                _TransactionRepository.Add(Newtransaction);
                _Resp.ReturnCode = Convert.ToInt16(enResponseCodeService.Success);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "exception,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                _Resp.ReturnCode = Convert.ToInt16(enResponseCodeService.InternalError);
                _Resp.ReturnMsg = ex.Message;
            }
                
            return _Resp;
        }

        
    }
}
