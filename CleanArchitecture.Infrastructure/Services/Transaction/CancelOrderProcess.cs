using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DTOClasses;
using CleanArchitecture.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class CancelOrderProcess : ICancelOrderProcess
    {
        private readonly ICommonRepository<TradeBuyRequest> _TradeBuyRequest;
        private readonly EFCommonRepository<TradeTransactionQueue> _TradeTransactionRepository;
        private readonly ISettlementRepository<BizResponse> _SettlementRepository;

        public CancelOrderProcess(ICommonRepository<TradeBuyRequest> TradeBuyRequest, EFCommonRepository<TradeTransactionQueue> TradeTransactionRepository, ISettlementRepository<BizResponse> SettlementRepository)
        {
            _TradeBuyRequest = TradeBuyRequest;
            _TradeTransactionRepository = TradeTransactionRepository;
            _SettlementRepository = SettlementRepository;
        }
        public async Task<BizResponse> ProcessCancelOrderAsync(CancelOrderRequest Req,string accessToken)
        {
            BizResponse _Resp = new BizResponse();
            try
            {
                var TradeTranQueueObj = _TradeTransactionRepository.GetSingle(item=>item.TrnNo==Req.TranNo);
                if(TradeTranQueueObj != null)
                {
                    _Resp.ErrorCode = enErrorCode.CancelOrder_NoRecordFound;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ReturnMsg ="No Record Found";
                    return _Resp;
                }
                if (TradeTranQueueObj.Status != Convert.ToInt16(enTransactionStatus.Hold))
                {
                    _Resp.ErrorCode = enErrorCode.CancelOrder_TrnNotHold;
                    _Resp.ReturnCode = enResponseCodeService.Fail;
                    _Resp.ReturnMsg = "Order is not in pending State";
                    return _Resp;
                }
                TradeTranQueueObj.IsCancelled = 1;
                _TradeTransactionRepository.Update(TradeTranQueueObj);

                var NewBuyRequestObj = _TradeBuyRequest.GetSingle(item => item.TrnNo == Req.TranNo && item.IsProcessing == 0 &&
                                                                    (item.Status == Convert.ToInt16(enTransactionStatus.Hold) ||
                                                                    item.Status == Convert.ToInt16(enTransactionStatus.Pending)));
                if (NewBuyRequestObj != null)
                {
                    var HoldTrnNosNotExec = new List<long> { };
                    _Resp = await _SettlementRepository.PROCESSSETLLEMENT(_Resp, NewBuyRequestObj, ref HoldTrnNosNotExec);
                }
                
            }
            catch(Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                _Resp.ErrorCode = enErrorCode.CancelOrder_InternalError;
                _Resp.ReturnCode = enResponseCodeService.Fail;
                _Resp.ReturnMsg = "Internal Error";
            }
            return _Resp;
        }
    }
}
