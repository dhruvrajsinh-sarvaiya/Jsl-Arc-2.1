using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    public class SignalRTestService : ISignalRTestService
    {
        private readonly EFCommonRepository<TransactionQueue> _TransactionRepository;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly IFrontTrnRepository _frontTrnRepository;

        public SignalRTestService(EFCommonRepository<TransactionQueue> TransactionRepository,
            IFrontTrnRepository frontTrnRepository,
            EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository)
        {
            _TransactionRepository = TransactionRepository;
            _frontTrnRepository = frontTrnRepository;
            _TradeTransactionRepository = TradeTransactionRepository;
        }
        public void MarkTransactionHold(long ID)
        {
            GetBuySellBook Buyermodel = new GetBuySellBook();
            GetBuySellBook Sellermodel = new GetBuySellBook();
            try
            {
                List<GetBuySellBook> list=new List<GetBuySellBook>();
                var Newtransaction = _TransactionRepository.GetById(ID);
                var NewTradeTransaction = _TradeTransactionRepository.GetById(Newtransaction.Id);
                Newtransaction.MakeTransactionHold();
                Newtransaction.SetTransactionStatusMsg("Hold");
                _TransactionRepository.Update(Newtransaction);

                NewTradeTransaction.Status = 4;
                NewTradeTransaction.StatusMsg = "Hold";
                _TradeTransactionRepository.Update(NewTradeTransaction);

                if(NewTradeTransaction .TrnType == 4)//Buy
                {
                    list = _frontTrnRepository.GetBuyerBook(NewTradeTransaction.PairID, NewTradeTransaction.BidPrice);
                    foreach (var model in list)
                    {
                        Buyermodel = model;
                        break;
                    }
                    
                }  
                else//Sell
                {
                    list = _frontTrnRepository.GetSellerBook(NewTradeTransaction.PairID, NewTradeTransaction.AskPrice);
                    foreach (var model in list)
                    {
                        Sellermodel = model;
                        break;
                    }
                }    
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog("MarkTransactionHold", ControllerName, ex);
                //throw ex;
            }
        }
    }
}
