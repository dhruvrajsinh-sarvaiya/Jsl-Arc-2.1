using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class FrontTrnService : IFrontTrnService
    {
        private readonly IFrontTrnRepository _frontTrnRepository;
        private readonly ICommonRepository<TradePairMaster> _tradeMasterRepository;
        private readonly ICommonRepository<TradePairDetail> _tradeDetailRepository;
        private readonly ICommonRepository<ServiceMaster> _serviceMasterRepository;
        private readonly ILogger<FrontTrnService> _logger;
        private readonly ICommonRepository<TradeTransactionQueue> _tradeTransactionQueueRepository;

        public FrontTrnService(IFrontTrnRepository frontTrnRepository, ICommonRepository<TradePairMaster> tradeMasterRepository, ICommonRepository<TradePairDetail> tradeDetailRepository, ILogger<FrontTrnService> logger, ICommonRepository<ServiceMaster> serviceMasterRepository, ICommonRepository<TradeTransactionQueue> tradeTransactionQueueRepository)
        {
            _frontTrnRepository = frontTrnRepository;
            _tradeMasterRepository = tradeMasterRepository;
            _tradeDetailRepository = tradeDetailRepository;
            _logger = logger;
            _serviceMasterRepository = serviceMasterRepository;
            _tradeTransactionQueueRepository = tradeTransactionQueueRepository;
        }
        
        public List<BasePairResponse> GetTradePairAsset()
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;

            List<BasePairResponse> responsedata;
            try
            {
                responsedata = new List<BasePairResponse>();
                var basePairData = _tradeMasterRepository.GetAll().GroupBy(x => x.BaseCurrencyId).Select(x => x.FirstOrDefault());

                if (basePairData != null)
                {
                    foreach (var bpair in basePairData)
                    {
                        BasePairResponse basePair = new BasePairResponse();
                        var baseService = _serviceMasterRepository.GetSingle(x => x.Id == bpair.BaseCurrencyId);

                        basePair.BaseCurrencyId = baseService.Id;
                        basePair.BaseCurrencyName = baseService.Name;
                        basePair.Abbrevation = baseService.SMSCode;

                        List<TradePairRespose> pairList = new List<TradePairRespose>();
                        var pairMasterData = _tradeMasterRepository.FindBy(x => x.BaseCurrencyId == bpair.BaseCurrencyId);
                        foreach (var pmdata in pairMasterData)
                        {
                            TradePairRespose tradePair = new TradePairRespose();
                            var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pmdata.Id);
                            var chidService = _serviceMasterRepository.GetSingle(x => x.Id == pmdata.SecondaryCurrencyId);

                            GetPairAdditionalVal(pmdata.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);

                            tradePair.PairId = pmdata.Id;
                            tradePair.Pairname = pmdata.PairName;
                            tradePair.Currentrate = pairDetailData.Currentrate;
                            tradePair.Volume = System.Math.Round(Volume24, 2);
                            tradePair.Fee = pairDetailData.Fee;
                            tradePair.ChildCurrency = chidService.Name;
                            tradePair.Abbrevation = chidService.SMSCode;
                            tradePair.ChangePer = System.Math.Round(ChangePer, 2);

                            pairList.Add(tradePair);
                        }
                        basePair.PairList = pairList;
                        responsedata.Add(basePair);
                    }
                    return responsedata;
                }
                else
                {
                    return responsedata;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public List<VolumeDataRespose> GetVolumeData()
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;

            List<VolumeDataRespose> responsedata;
            try
            {
                responsedata = new List<VolumeDataRespose>();
                var pairMasterData = _tradeMasterRepository.GetAll();

                if(pairMasterData != null)
                {
                    foreach (var pmdata in pairMasterData)
                    {
                        VolumeDataRespose volumedata = new VolumeDataRespose();
                        var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pmdata.Id);
                        GetPairAdditionalVal(pmdata.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);

                        volumedata.PairId = pmdata.Id;
                        volumedata.Currentrate = pairDetailData.Currentrate;
                        volumedata.ChangePer = System.Math.Round(Volume24, 2);
                        volumedata.Volume24 = System.Math.Round(ChangePer, 2);

                        responsedata.Add(volumedata);
                    }
                    return responsedata;
                }
                else
                {
                    return responsedata;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public void GetPairAdditionalVal(long PairId, decimal CurrentRate, ref decimal Volume24, ref decimal ChangePer)
        {
            try
            {
                //Calucalte ChangePer

                decimal tradeprice = 0;
                var tradedata = _tradeTransactionQueueRepository.GetSingle(x => x.TrnDate > DateTime.Now.AddDays(-1) && x.PairID == PairId && x.Status == 1);
                if (tradedata != null)
                {
                    if (tradedata.TrnType == 4)
                    {
                        tradeprice = tradedata.BidPrice;
                    }
                    else if (tradedata.TrnType == 5)
                    {
                        tradeprice = tradedata.AskPrice;
                    }
                    if (CurrentRate > 0 && tradeprice > 0)
                        ChangePer = ((CurrentRate * 100) / tradeprice) - 100;
                    else if (CurrentRate > 0 && tradeprice == 0)
                    {
                        ChangePer = 100;
                    }
                    else
                        ChangePer = 0;
                }
                else
                {
                    ChangePer = 0;
                }

                //Calculate Volume24
                tradeprice = 0;
                decimal tradeqty = 0, sum = 0;
                var tradedata1 = _tradeTransactionQueueRepository.FindBy(x => x.TrnDate >= DateTime.Now.AddDays(-1) && x.TrnDate <= DateTime.Now && x.PairID == PairId && x.Status == 1 && (x.TrnType == 4 || x.TrnType == 5));
                if (tradedata1 != null)
                {
                    foreach (var trade in tradedata1)
                    {
                        if (trade.TrnType == 4)
                        {
                            tradeprice = trade.BidPrice;
                            tradeqty = trade.BuyQty;
                        }
                        else if (trade.TrnType == 5)
                        {
                            tradeprice = trade.AskPrice;
                            tradeqty = trade.SellQty;
                        }
                        else
                        {
                            tradeprice = 0;
                            tradeqty = 0;
                        }
                        sum += (tradeprice * tradeqty);
                    }
                    Volume24 = sum;
                }
                else
                {
                    Volume24 = 0;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public List<GetTradeHistoryInfo> GetTradeHistory(long MemberID, long PairId, int IsAll)
        {
            try
            {
                var list = _frontTrnRepository.GetTradeHistory(MemberID, PairId, IsAll);
                List<GetTradeHistoryInfo> responce = new List<GetTradeHistoryInfo>();
                if (list != null)
                {
                    foreach (TradeHistoryResponce model in list)
                    {
                        responce.Add(new GetTradeHistoryInfo
                        {
                            Amount = model.Amount,
                            ChargeRs = model.ChargeRs,
                            DateTime = model.DateTime.Date,
                            PairID = model.PairID,
                            Price = model.Price,
                            Status = model.Status,
                            StatusText = model.StatusText,
                            TrnNo = model.TrnNo,
                            Type = model.Type,
                            Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount))
                        });
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }

        }
        public List<RecentOrderInfo> GetRecentOrder(long PairId)
        {
            try
            {
                var list = _frontTrnRepository.GetRecentOrder(PairId);
                List<RecentOrderInfo> responce = new List<RecentOrderInfo>();
                if (list != null)
                {
                    foreach (RecentOrderRespose model in list)
                    {
                        responce.Add(new RecentOrderInfo
                        {
                            Qty = model.Qty,
                            DateTime = model.DateTime.Date,
                            Price = model.Price,
                            Status = model.Status,
                            TrnNo = model.TrnNo,
                            Type = model.Type,
                        });
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public List<ActiveOrderInfo> GetActiveOrder(long MemberID, long PairId)
        {
            try
            {
                List<ActiveOrderDataResponse> ActiveOrderList = _frontTrnRepository.GetActiveOrder(MemberID, PairId);
                List<ActiveOrderInfo> responceData = new List<ActiveOrderInfo>();
                if (ActiveOrderList != null)
                {
                    foreach (ActiveOrderDataResponse model in ActiveOrderList)
                    {
                        responceData.Add(new ActiveOrderInfo
                        {
                            Amount = model.Amount,
                            Delivery_Currency = model.Delivery_Currency,
                            Id = model.Id,
                            IsCancelled = model.IsCancelled,
                            Order_Currency = model.Order_Currency,
                            Price = model.Price,
                            TrnDate = model.TrnDate.Date,
                            Type = model.Type
                        });
                    }
                }
                return responceData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public long GetPairIdByName(string pair)
        {
            try
            {
                return _frontTrnRepository.GetPairIdByName(pair);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public bool IsValidPairName(string Pair)
        {
            try
            {
                String Pattern = "^[A-Z]{6,9}$";
                if(Regex.IsMatch(Pair, Pattern, RegexOptions.IgnoreCase))
                      return true;

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
