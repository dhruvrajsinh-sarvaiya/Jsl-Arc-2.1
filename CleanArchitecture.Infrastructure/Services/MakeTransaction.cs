using System;
using System.Collections.Generic;
using System.Text;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Infrastructure.Services
{
    public class MakeTransaction
    {
        private readonly IRepository<TransactionQueue> _TransactionRepository;

        public MakeTransaction(IRepository<TransactionQueue> TransactionRepository)
        {
            _TransactionRepository = TransactionRepository;
        }

        public string CreateTransaction(TransactionQueue NewtransactionReq)
        {
            var Newtransaction = new TransactionQueue()
            {
                TrnDate = DateTime.UtcNow,
                TrnMode = NewtransactionReq.TrnMode,
                TrnType = NewtransactionReq.TrnType,
                MemberID = NewtransactionReq.MemberID,
                MemberMobile = NewtransactionReq.MemberMobile,
                SMSText = NewtransactionReq.SMSText,
                SMSCode = NewtransactionReq.SMSCode,
                Amount = NewtransactionReq.Amount,
                SMSPwd = NewtransactionReq.SMSPwd,
                ServiceID = NewtransactionReq.ServiceID,
                SerProID = NewtransactionReq.SerProID,
                ProductID = NewtransactionReq.ProductID,
                RoutID = NewtransactionReq.RoutID,
                Status = NewtransactionReq.Status,
                StatusCode  = NewtransactionReq.StatusCode,
                StatusMsg = NewtransactionReq.StatusMsg,
                VerifyDone =0,
                TrnRefNo = NewtransactionReq.TrnRefNo,
                ChargePer = NewtransactionReq.ChargePer,
                ChargeRs = NewtransactionReq.ChargeRs,
                ChargeType = NewtransactionReq.ChargeType,
            };
            _TransactionRepository.Add(Newtransaction);
            return "Success";
        }
    }
}
