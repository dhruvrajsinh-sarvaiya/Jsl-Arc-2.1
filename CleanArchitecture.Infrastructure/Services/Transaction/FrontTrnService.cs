using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Communication;
using CleanArchitecture.Core.Entities.Configuration;
using CleanArchitecture.Core.Entities.Transaction;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Repository;
using CleanArchitecture.Core.ViewModels;
using CleanArchitecture.Core.ViewModels.Transaction;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
        private readonly ICommonRepository<SettledTradeTransactionQueue> _settelTradeTranQueue;
        private readonly ICommonRepository<TradePairStastics> _tradePairStastics;
        private readonly ICommonRepository<Market> _marketRepository;
        private readonly ICommonRepository<FavouritePair> _favouritePairRepository;
        private readonly IBasePage _basePage;
        private readonly ICommonRepository<TradeGraphDetail> _graphDetailRepository;
        private readonly ISignalRService _signalRService;

        public FrontTrnService(IFrontTrnRepository frontTrnRepository,
            ICommonRepository<TradePairMaster> tradeMasterRepository,
            ICommonRepository<TradePairDetail> tradeDetailRepository,
            ILogger<FrontTrnService> logger,
            ICommonRepository<ServiceMaster> serviceMasterRepository,
            ICommonRepository<TradeTransactionQueue> tradeTransactionQueueRepository,
            ICommonRepository<SettledTradeTransactionQueue> settelTradeTranQueue,
            ICommonRepository<TradePairStastics> tradePairStastics,
            ICommonRepository<Market> marketRepository,
            ICommonRepository<FavouritePair> favouritePairRepository,
            IBasePage basePage,
            ICommonRepository<TradeGraphDetail> graphDetailRepository,
            ISignalRService signalRService)

        {
            _frontTrnRepository = frontTrnRepository;
            _tradeMasterRepository = tradeMasterRepository;
            _tradeDetailRepository = tradeDetailRepository;
            _logger = logger;
            _serviceMasterRepository = serviceMasterRepository;
            _tradeTransactionQueueRepository = tradeTransactionQueueRepository;
            _settelTradeTranQueue = settelTradeTranQueue;
            _tradePairStastics = tradePairStastics;
            _marketRepository = marketRepository;
            _favouritePairRepository = favouritePairRepository;
            _basePage = basePage;
            _graphDetailRepository = graphDetailRepository;
            _signalRService = signalRService;
        }

        #region method

        public List<BasePairResponse> GetTradePairAsset()
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;

            List<BasePairResponse> responsedata;
            try
            {
                responsedata = new List<BasePairResponse>();
                var basePairData = _marketRepository.GetAll();
                

                if (basePairData != null)
                {
                    foreach (var bpair in basePairData)
                    {
                        BasePairResponse basePair = new BasePairResponse();
                        var TradePairList = _frontTrnRepository.GetTradePairAsset();

                        //var baseService = _serviceMasterRepository.GetSingle(x => x.Id == bpair.ServiceID);

                        var pairData = TradePairList.Where(x => x.BaseId == bpair.ServiceID);
                        if (pairData.Count() > 0)
                        {
                            basePair.BaseCurrencyId = pairData.FirstOrDefault().BaseId;
                            basePair.BaseCurrencyName = pairData.FirstOrDefault().BaseName;
                            basePair.Abbrevation = pairData.FirstOrDefault().BaseCode;

                            List<TradePairRespose> pairList = new List<TradePairRespose>();
                            //var pairMasterData = _tradeMasterRepository.FindBy(x => x.BaseCurrencyId == bpair.ServiceID);
                            foreach (var pair in pairData)
                            {
                                TradePairRespose tradePair = new TradePairRespose();
                               // var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pmdata.Id);
                               // var chidService = _serviceMasterRepository.GetSingle(x => x.Id == pmdata.SecondaryCurrencyId);
                               // var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pmdata.Id);
                                //GetPairAdditionalVal(pmdata.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);

                                tradePair.PairId = pair.PairId;
                                tradePair.Pairname = pair.Pairname;
                                tradePair.Currentrate = pair.Currentrate;
                                tradePair.BuyFees = pair.BuyFees;
                                tradePair.SellFees = pair.SellFees;
                                tradePair.ChildCurrency = pair.ChildCurrency;
                                tradePair.Abbrevation = pair.Abbrevation;
                                //tradePair.ChangePer = System.Math.Round(ChangePer, 2);
                                //tradePair.Volume = System.Math.Round(Volume24, 2);
                                tradePair.ChangePer = pair.ChangePer;
                                tradePair.Volume = pair.Volume;
                                tradePair.High24Hr = pair.High24Hr;
                                tradePair.Low24Hr = pair.Low24Hr;
                                tradePair.HighWeek = pair.HighWeek;
                                tradePair.LowWeek = pair.LowWeek;
                                tradePair.High52Week = pair.High52Week;
                                tradePair.Low52Week = pair.Low52Week;
                                tradePair.UpDownBit = pair.UpDownBit;

                                pairList.Add(tradePair);
                            }
                            basePair.PairList = pairList;
                            responsedata.Add(basePair);
                        }
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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<VolumeDataRespose> GetVolumeData(long BasePairId)
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;

            List<VolumeDataRespose> responsedata;
            try
            {
                responsedata = new List<VolumeDataRespose>();
                //var pairMasterData = _tradeMasterRepository.FindBy(x => x.BaseCurrencyId == BasePairId);
                var TradePairList = _frontTrnRepository.GetTradePairAsset();
                var pairMasterData = TradePairList.Where(x => x.BaseId == BasePairId).ToList();

                if (pairMasterData != null && pairMasterData.Count() > 0)
                {
                    foreach (var pmdata in pairMasterData)
                    {
                        VolumeDataRespose volumedata = new VolumeDataRespose();
                        //var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pmdata.Id);
                        //var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pmdata.Id);
                        //GetPairAdditionalVal(pmdata.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);


                        volumedata.PairId = pmdata.PairId;
                        volumedata.PairName = pmdata.Pairname;
                        volumedata.Currentrate = pmdata.Currentrate;
                        //volumedata.ChangePer = System.Math.Round(Volume24, 2);
                        //volumedata.Volume24 = System.Math.Round(ChangePer, 2);
                        volumedata.ChangePer = pmdata.ChangePer;
                        volumedata.Volume24 = pmdata.Volume;
                        volumedata.High24Hr = pmdata.High24Hr;
                        volumedata.Low24Hr = pmdata.Low24Hr;
                        volumedata.HighWeek = pmdata.HighWeek;
                        volumedata.LowWeek = pmdata.LowWeek;
                        volumedata.High52Week = pmdata.High52Week;
                        volumedata.Low52Week = pmdata.Low52Week;
                        volumedata.UpDownBit = pmdata.UpDownBit;

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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //public void GetPairAdditionalVal(long PairId, decimal CurrentRate,long TrnNo)
        //{
        //    try
        //    {
        //        //Calucalte ChangePer
        //        decimal Volume24 = 0, ChangePer = 0,High24Hr=0,Low24Hr=0;
        //        short UpDownBit = 0;
        //        decimal tradeprice = 0,todayopen,todayclose;
        //        var tradedata = _tradeTransactionQueueRepository.GetSingle(x => x.TrnDate > _basePage.UTC_To_IST().AddDays(-1) && x.PairID == PairId && x.Status == 1);
        //        if (tradedata != null)
        //        {
        //            if (tradedata.TrnType == 4)
        //            {
        //                tradeprice = tradedata.BidPrice;
        //            }
        //            else if (tradedata.TrnType == 5)
        //            {
        //                tradeprice = tradedata.AskPrice;
        //            }
        //            if (CurrentRate > 0 && tradeprice > 0)
        //                ChangePer = ((CurrentRate * 100) / tradeprice) - 100;
        //            else if (CurrentRate > 0 && tradeprice == 0)
        //            {
        //                ChangePer = 100;
        //            }
        //            else
        //                ChangePer = 0;
        //        }
        //        else
        //        {
        //            ChangePer = 0;
        //        }

        //        //Calculate Volume24
        //        tradeprice = 0;
        //        decimal tradeqty = 0, sum = 0;
        //        var tradedata1 = _tradeTransactionQueueRepository.FindBy(x => x.TrnDate >= _basePage.UTC_To_IST().AddDays(-1) && x.TrnDate <= DateTime.Now && x.PairID == PairId && x.Status == 1 && (x.TrnType == 4 || x.TrnType == 5));
        //        if (tradedata1 != null)
        //        {
        //            foreach (var trade in tradedata1)
        //            {
        //                if (trade.TrnType == 4)
        //                {
        //                    tradeprice = trade.BidPrice;
        //                    tradeqty = trade.BuyQty;
        //                }
        //                else if (trade.TrnType == 5)
        //                {
        //                    tradeprice = trade.AskPrice;
        //                    tradeqty = trade.SellQty;
        //                }
        //                else
        //                {
        //                    tradeprice = 0;
        //                    tradeqty = 0;
        //                }
        //                sum += (tradeprice * tradeqty);
        //            }
        //            Volume24 = sum;
        //        }
        //        else
        //        {
        //            Volume24 = 0;
        //        }

        //        //Calculate High24Hr,Low24Hr,UpDownBit
        //        var pairData = _tradePairStastics.GetSingle(x => x.PairId == PairId);
        //        if(pairData.High24Hr == 0 && pairData.Low24Hr == 0 && pairData.UpDownBit == 0)
        //        {
        //            High24Hr = CurrentRate;
        //            Low24Hr = CurrentRate;
        //            UpDownBit = 1;
        //        }
        //        else
        //        {
        //            if(CurrentRate > pairData.High24Hr)
        //            {
        //                High24Hr = CurrentRate;
        //                Low24Hr = pairData.Low24Hr;
        //                UpDownBit = 1;
        //            }
        //            else if(CurrentRate < pairData.Low24Hr)
        //            {
        //                Low24Hr = CurrentRate;
        //                High24Hr = pairData.High24Hr;
        //                UpDownBit = 0;
        //            }
        //            else
        //            {
        //                Low24Hr = pairData.Low24Hr;
        //                High24Hr = pairData.High24Hr;
        //                if(CurrentRate < pairData.LTP)
        //                {
        //                    UpDownBit = 0;
        //                }
        //                else if(CurrentRate > pairData.LTP)
        //                {
        //                    UpDownBit = 1;
        //                }
        //            }
        //        }

        //        //Update Pair Statstics Data
        //        pairData.ChangePer24 = ChangePer;
        //        pairData.ChangeVol24 = Volume24;
        //        pairData.High24Hr = High24Hr;
        //        pairData.Low24Hr = Low24Hr;
        //        pairData.UpDownBit = UpDownBit;
        //        pairData.LTP = CurrentRate;
        //        pairData.CurrentRate = CurrentRate;
        //        _tradePairStastics.Update(pairData);


        //        //Calculate TodayOpen,TodayClose
        //        var now = _basePage.UTC_To_IST();
        //        DateTime startDateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
        //        DateTime endDateTime = _basePage.UTC_To_IST();

        //        var tradegraphdetail = _graphDetailRepository.FindBy(x => x.DataDate >= startDateTime && x.DataDate <= endDateTime).OrderBy(x => x.TranNo).FirstOrDefault();
        //        if(tradegraphdetail != null)
        //        {
        //            todayopen = tradegraphdetail.LTP;
        //            todayclose = CurrentRate;
        //        }
        //        else
        //        {
        //            todayopen = CurrentRate;
        //            todayclose = CurrentRate;
        //        }

        //        //Add Data Into Graph Table
        //        var tradegraph = new TradeGraphDetail()
        //        {
        //            PairId = PairId,
        //            TranNo = TrnNo,
        //            DataDate = _basePage.UTC_To_IST(),
        //            ChangePer = ChangePer,
        //            Volume = Volume24,
        //            High24Hr = High24Hr,
        //            Low24Hr = Low24Hr,
        //            LTP = CurrentRate,
        //            TodayOpen = todayopen,
        //            TodayClose = todayclose,
        //            CreatedBy = 1,
        //            CreatedDate = _basePage.UTC_To_IST()
        //        };
        //        _graphDetailRepository.Add(tradegraph);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}
        public async void GetPairAdditionalVal(long PairId, decimal CurrentRate, long TrnNo, decimal Quantity, DateTime TranDate)
        {
            try
            {
                //Calucalte ChangePer
                decimal Volume24 = 0, ChangePer = 0, High24Hr = 0, Low24Hr = 0, WeekHigh = 0, WeekLow = 0, Week52High = 0, Week52Low = 0;
                short UpDownBit = 1; //komal 13-11-2018 set defau
                decimal tradeprice = 0, todayopen, todayclose;
                decimal LastRate = 0;

                var tradeRateData = _tradeTransactionQueueRepository.FindBy(x => x.TrnDate > _basePage.UTC_To_IST().AddDays(-1) && x.PairID == PairId && x.Status == 1 && (x.TrnType == Convert.ToInt16(enTrnType.Sell_Trade) || x.TrnType == Convert.ToInt16(enTrnType.Buy_Trade))).OrderByDescending(x => x.TrnNo).FirstOrDefault();
                if (tradeRateData != null)
                {
                    if (tradeRateData.TrnType == Convert.ToInt16(enTrnType.Buy_Trade))
                    {
                        LastRate = tradeRateData.BidPrice;
                    }
                    else if (tradeRateData.TrnType == Convert.ToInt16(enTrnType.Sell_Trade))
                    {
                        LastRate = tradeRateData.AskPrice;
                    }
                }
                else
                {
                    LastRate = 0;
                }

                var tradedata = _tradeTransactionQueueRepository.GetSingle(x => x.TrnDate > _basePage.UTC_To_IST().AddDays(-1) && x.PairID == PairId && x.Status == 1);
                if (tradedata != null)
                {
                    HelperForLog.WriteLogIntoFile("#GetPairAdditionalVal# #CHANGEPER# #Count# : 1 " + " #TrnNo# :" + TrnNo + " #BidPrice# : " + tradedata.BidPrice + " #AskPrice# : " + tradedata.AskPrice, "FrontService", "Object Data : ");
                    if (tradedata.TrnType == 4)
                    {
                        tradeprice = tradedata.BidPrice;
                    }
                    else if (tradedata.TrnType == 5)
                    {
                        tradeprice = tradedata.AskPrice;
                    }
                    if (LastRate > 0 && tradeprice > 0)
                        ChangePer = ((LastRate * 100) / tradeprice) - 100;
                    else if (LastRate > 0 && tradeprice == 0)
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
                var tradedata1 = _tradeTransactionQueueRepository.FindBy(x => x.TrnDate >= _basePage.UTC_To_IST().AddDays(-1) && x.TrnDate <= _basePage.UTC_To_IST() && x.PairID == PairId && x.Status == 1 && (x.TrnType == 4 || x.TrnType == 5));
                if (tradedata1 != null)
                {

                    foreach (var trade in tradedata1)
                    {
                        HelperForLog.WriteLogIntoFile("#GetPairAdditionalVal# #VolumeData#  " + " #TrnNo# :" + trade.TrnNo + " #BidPrice# : " + trade.BidPrice + " #AskPrice# : " + trade.AskPrice, "FrontService", "Object Data : ");
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

                HelperForLog.WriteLogIntoFile("#GetPairAdditionalVal# " + " #VolumeData :" + Volume24 + " #ChangePer# : " + ChangePer, "FrontService", "Object Data : ");

                //Insert In GraphDetail Only BidPrice
                var DataDate = TranDate;
                var tradegraph = new TradeGraphDetail()
                {
                    PairId = PairId,
                    TranNo = TrnNo,
                    DataDate = DataDate,
                    ChangePer = ChangePer,
                    Volume = Volume24,
                    BidPrice = CurrentRate,
                    LTP = CurrentRate,
                    Quantity = Quantity,
                    CreatedBy = 1,
                    CreatedDate = _basePage.UTC_To_IST()
                };

                try
                {
                    tradegraph = _graphDetailRepository.Add(tradegraph);
                }
                catch (Exception ex)
                {
                    HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                }
                finally
                {
                    //Calculate High Low Data For 24Hr
                    var tardeTrabDetail = _tradeTransactionQueueRepository.FindBy(x => x.PairID == PairId && x.TrnDate >= _basePage.UTC_To_IST().AddDays(-1) && x.TrnDate <= _basePage.UTC_To_IST()).OrderBy(x => x.Id).ToList();
                    High24Hr = LastRate;
                    Low24Hr = LastRate;
                    if (tardeTrabDetail.Count > 0)
                    {
                        foreach (TradeTransactionQueue type in tardeTrabDetail)
                        {
                            decimal price = 0;
                            if (type.TrnType == Convert.ToInt16(enTrnType.Buy_Trade))
                            {
                                price = type.BidPrice;
                            }
                            else if (type.TrnType == Convert.ToInt16(enTrnType.Sell_Trade))
                            {
                                price = type.AskPrice;
                            }

                            if (price > High24Hr)
                            {
                                High24Hr = price;
                            }
                            if (price < Low24Hr)
                            {
                                Low24Hr = price;
                            }
                        }
                    }

                    ////Calculate High Low Data For Week
                    //var tradegraphdetail2 = _graphDetailRepository.FindBy(x => x.DataDate >= _basePage.UTC_To_IST().AddDays(-7) && x.DataDate <= _basePage.UTC_To_IST()).OrderBy(x => x.TranNo).ToList();
                    //WeekHigh = CurrentRate;
                    //WeekLow = CurrentRate;
                    //if (tradegraphdetail2.Count > 0)
                    //{
                    //    foreach (TradeGraphDetail type in tradegraphdetail2)
                    //    {
                    //        if (type.BidPrice > WeekHigh)
                    //        {
                    //            WeekHigh = type.BidPrice;
                    //        }
                    //        if (type.BidPrice < WeekLow)
                    //        {
                    //            WeekLow = type.BidPrice;
                    //        }
                    //    }
                    //}

                    ////Calculate High Low Data For 52Week
                    //var tradegraphdetail3 = _graphDetailRepository.FindBy(x => x.DataDate >= _basePage.UTC_To_IST().AddDays(-365) && x.DataDate <= _basePage.UTC_To_IST()).OrderBy(x => x.TranNo).ToList();
                    //Week52High = CurrentRate;
                    //Week52Low = CurrentRate;
                    //if (tradegraphdetail3.Count > 0)
                    //{
                    //    foreach (TradeGraphDetail type in tradegraphdetail2)
                    //    {
                    //        if (type.BidPrice > Week52High)
                    //        {
                    //            Week52High = type.BidPrice;
                    //        }
                    //        if (type.BidPrice < Week52Low)
                    //        {
                    //            Week52Low = type.BidPrice;
                    //        }
                    //    }
                    //}              

                    ////Calculate Open Close
                    //var now = _basePage.UTC_To_IST();
                    //DateTime startDateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                    //DateTime endDateTime = _basePage.UTC_To_IST();

                    //var tradegraphdetail1 = _graphDetailRepository.FindBy(x => x.DataDate >= startDateTime && x.DataDate <= endDateTime).OrderBy(x => x.TranNo).FirstOrDefault();
                    //if (tradegraphdetail1 != null)
                    //{
                    //    todayopen = tradegraphdetail1.BidPrice;
                    //    todayclose = CurrentRate;
                    //}
                    //else
                    //{
                    //    todayopen = CurrentRate;
                    //    todayclose = CurrentRate;
                    //}


                    ////Update TradeGraph Detail Data

                    //tradegraph.High24Hr = High24Hr;
                    //tradegraph.Low24Hr = Low24Hr;
                    //tradegraph.HighWeek = WeekHigh;
                    //tradegraph.LowWeek = WeekLow;
                    //tradegraph.High52Week = Week52High;
                    //tradegraph.Low52Week = Week52Low;
                    //tradegraph.LTP = CurrentRate;
                    //tradegraph.TodayOpen = todayopen;
                    //tradegraph.TodayClose = todayclose;                   
                    //_graphDetailRepository.Update(tradegraph);

                    //Uday 14-11-2018 CurrentRate Calculation Changes based on TrnDate

                    var pairData = _tradePairStastics.GetSingle(x => x.PairId == PairId);
                    if (TranDate > pairData.TranDate)
                    {
                        pairData.LTP = CurrentRate;
                        pairData.CurrentRate = CurrentRate;
                    }
                    else
                    {
                        CurrentRate = pairData.CurrentRate;
                    }
                    _tradePairStastics.Update(pairData);

                    if (CurrentRate > pairData.High24Hr) //komal 13-11-2018 Change code sequence cos got 0 every time
                    {
                        UpDownBit = 1;
                    }
                    else if (CurrentRate < pairData.Low24Hr)
                    {
                        UpDownBit = 0;
                    }
                    else
                    {
                        if (CurrentRate < pairData.LTP)
                        {
                            UpDownBit = 0;
                        }
                        else if (CurrentRate > pairData.LTP)
                        {
                            UpDownBit = 1;
                        }
                        else if (CurrentRate == pairData.LTP)//komal 13-11-2018 if no change then set as it is
                        {
                            UpDownBit = pairData.UpDownBit;
                        }
                    }
                    pairData.ChangePer24 = ChangePer;
                    pairData.ChangeVol24 = Volume24;
                    pairData.High24Hr = High24Hr;
                    pairData.Low24Hr = Low24Hr;
                    pairData.LTP = CurrentRate;
                    pairData.CurrentRate = CurrentRate;
                    pairData.HighWeek = WeekHigh;
                    pairData.LowWeek = WeekLow;
                    pairData.High52Week = Week52High;
                    pairData.Low52Week = Week52Low;
                    _tradePairStastics.Update(pairData);

                    var VolumeData = GetVolumeDataByPair(PairId);
                    var MarketData = GetMarketCap(PairId);
                    var GraphDataList = _frontTrnRepository.GetGraphData(PairId, 1, "MINUTE", DataDate, 1);
                    if (GraphDataList.Count() > 0)
                    {
                        DateTime dt2 = new DateTime(1970, 1, 1);
                        List<GetGraphDetailInfo> responseData = new List<GetGraphDetailInfo>();

                        //responseData = GraphDataList.Select(a => new GetGraphDetailInfo()
                        //{
                        //    DataDate = Convert.ToInt64(a.DataDate.Subtract(dt2).TotalMilliseconds),
                        //    High = a.High,
                        //    Low = a.Low,
                        //    Open = a.OpenVal,
                        //    Close = a.OpenVal,
                        //    Volume = a.Volume,
                        //}).ToList();

                        var GraphData = responseData.FirstOrDefault();
                        HelperForLog.WriteLogIntoFile("#GraphDataToSocket# #TrnNo# : " + TrnNo, "FrontService", "Object Data : ");
                        _signalRService.ChartData(GraphData, VolumeData.PairName);
                    }

                    HelperForLog.WriteLogIntoFile("#VolumeDataToSocket# #PairId# : " + PairId, "FrontService", "Object Data : ");
                    HelperForLog.WriteLogIntoFile("#MarketDataToSocket# #PairId# : " + PairId, "FrontService", "Object Data : ");
                    _signalRService.OnVolumeChange(VolumeData, MarketData);
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<GetTradeHistoryInfo> GetTradeHistory(long MemberID, string sCondition, string FromDate, string TodDate, int page, int IsAll)
        {
            try
            {
                List<TradeHistoryResponce> list = _frontTrnRepository.GetTradeHistory(MemberID, sCondition, FromDate, TodDate, page, IsAll);
                List<GetTradeHistoryInfo> responce = new List<GetTradeHistoryInfo>();
                
                if (list != null)
                {
                    if(page > 0)
                    {
                        int skip = Helpers.PageSize * (page - 1);
                        list = list.Skip(skip).Take(Helpers.PageSize).ToList();
                    }
                    
                    foreach (TradeHistoryResponce model in list)
                    {
                        responce.Add(new GetTradeHistoryInfo
                        {
                            Amount = model.Amount,
                            ChargeRs = model.ChargeRs,
                            DateTime = model.DateTime,
                            PairName = model.PairName,
                            Price = model.Price,
                            Status = model.Status,
                            StatusText = model.StatusText,
                            TrnNo = model.TrnNo,
                            Type = model.Type,
                            Total = model.Type == "BUY" ? ((model.Price * model.Amount) - model.ChargeRs) : ((model.Price * model.Amount)),
                            IsCancel=model .IsCancelled,
                            OrderType = Enum.GetName(typeof(enTransactionMarketType), model.ordertype)

                        });
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }
        public List<RecentOrderInfo> GetRecentOrder(long PairId, long MemberID)
        {
            try
            {
                string st = "";
                var list = _frontTrnRepository.GetRecentOrder(PairId, MemberID);
                List<RecentOrderInfo> responce = new List<RecentOrderInfo>();
                if (list != null)
                {
                    foreach (RecentOrderRespose model in list)
                    {
                        if (model.Status == Convert.ToInt16(enTransactionStatus.Success))
                            st = "Success";
                        else if (model.Status == Convert.ToInt16(enTransactionStatus.Hold))
                            st = "Hold";
                        else if (model.Status == Convert.ToInt16(enTransactionStatus.SystemFail))
                            st = "Fail";

                        responce.Add(new RecentOrderInfo
                        {
                            Qty = model.Qty,
                            DateTime = model.DateTime,
                            Price = model.Price,
                            TrnNo = model.TrnNo,
                            Type = model.Type,
                            Status = st,
                            PairId=model.PairId,
                            PairName=model.PairName,
                            OrderType = Enum.GetName(typeof(enTransactionMarketType), model.ordertype)
                        });
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<ActiveOrderInfo> GetActiveOrder(long MemberID, string sCondition, string FromDate, string TodDate, long PairId, int Page)
        {
            try
            {
                List<ActiveOrderDataResponse> ActiveOrderList = _frontTrnRepository.GetActiveOrder(MemberID,sCondition ,FromDate ,TodDate, PairId);
                List<ActiveOrderInfo> responceData = new List<ActiveOrderInfo>();
                if (ActiveOrderList != null)
                {
                    if(Page > 0)
                    {
                        int skip = Helpers.PageSize * (Page - 1);
                        ActiveOrderList = ActiveOrderList.Skip(skip).Take(Helpers.PageSize).ToList();
                    }
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
                            TrnDate = model.TrnDate,
                            Type = model.Type,
                            PairName =model.PairName,
                            PairId=model.PairId,
                            OrderType= Enum.GetName(typeof(enTransactionMarketType),model.ordertype)

                    });
                    }
                }
                return responceData;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<GetBuySellBook> GetBuyerBook(long id)
        {
            try
            {
                var list = _frontTrnRepository.GetBuyerBook(id);
                List<GetBuySellBook> responce = new List<GetBuySellBook>();
                if (list != null)
                {
                    foreach(var data in list)
                    {
                        if(data.Price != 0 && data.Amount != 0)
                        {
                            responce.Add(data);
                        }
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<GetBuySellBook> GetSellerBook(long id)
        {
            try
            {
                var list = _frontTrnRepository.GetSellerBook(id);
                List<GetBuySellBook> responce = new List<GetBuySellBook>();
                if (list != null)
                {
                    foreach (var data in list)
                    {
                        if (data.Price != 0 && data.Amount != 0)
                        {
                            responce.Add(data);
                        }
                    }
                }
                return responce;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public GetTradePairByName GetTradePairByName(long id)
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;
            GetTradePairByName responsedata = new GetTradePairByName();
            try
            {
                var pairMasterData = _tradeMasterRepository.GetById(id);
                if (pairMasterData != null)
                {

                    var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pairMasterData.Id);
                    var baseService = _serviceMasterRepository.GetSingle(x => x.Id == pairMasterData.BaseCurrencyId);
                    var chidService = _serviceMasterRepository.GetSingle(x => x.Id == pairMasterData.SecondaryCurrencyId);
                    var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pairMasterData.Id);
                    //GetPairAdditionalVal(pairMasterData.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);

                    responsedata.PairId = pairMasterData.Id;
                    responsedata.Pairname = pairMasterData.PairName;
                    responsedata.Currentrate = pairStastics.CurrentRate;
                    responsedata.BuyFees = pairDetailData.BuyFees;
                    responsedata.SellFees = pairDetailData.SellFees;
                    responsedata.ChildCurrency = chidService.Name;
                    responsedata.Abbrevation = chidService.SMSCode;
                    //tradePair.ChangePer = System.Math.Round(ChangePer, 2);
                    //tradePair.Volume = System.Math.Round(Volume24, 2);
                    responsedata.ChangePer = pairStastics.ChangePer24;
                    responsedata.Volume = pairStastics.ChangeVol24;
                    responsedata.High24Hr = pairStastics.High24Hr;
                    responsedata.Low24Hr = pairStastics.Low24Hr;
                    responsedata.HighWeek = pairStastics.HighWeek;
                    responsedata.LowWeek = pairStastics.LowWeek;
                    responsedata.High52Week = pairStastics.High52Week;
                    responsedata.Low52Week = pairStastics.Low52Week;
                    responsedata.UpDownBit = pairStastics.UpDownBit;
                   
                    responsedata.BaseCurrencyId = baseService.Id;
                    responsedata.BaseCurrencyName = baseService.Name;
                    responsedata.BaseAbbrevation = baseService.SMSCode;
                }
                return responsedata;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public long GetBasePairIdByName(string BasePair)
        {
            try
            {
                var model = _serviceMasterRepository.GetSingle(x => x.SMSCode == BasePair);
                if (model == null)
                    return 0;

                return model.Id;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<GetGraphDetailInfo> GetGraphDetail(long PairId, int IntervalTime, string IntervalData)
        {
            try
            {
                List<GetGraphDetailInfo> responseData = new List<GetGraphDetailInfo>();
                var list = _frontTrnRepository.GetGraphData(PairId,IntervalTime,IntervalData,_basePage.UTC_To_IST()).OrderBy(x => x.DataDate);

                //Uday 14-11-2018 Direct Query On Absolute View So No conversion required
                //DateTime dt2 = new DateTime(1970, 1, 1);
                //responseData = list.Select(a => new GetGraphDetailInfo()
                //{
                //    DataDate = Convert.ToInt64(a.DataDate.Subtract(dt2).TotalMilliseconds),
                //    High = a.High,
                //    Low = a.Low,
                //    Open = a.OpenVal,
                //    Close = a.OpenVal,
                //    Volume = a.Volume,
                //}).ToList();

                //return responseData;

                return list.ToList();
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }


        public MarketCapData GetMarketCap(long PairId)
        {
            try
            {
                MarketCapData dataRes = new MarketCapData();
                VolumeDataRespose res = new VolumeDataRespose();
                var pairMasterData = _tradeMasterRepository.GetById(PairId);
                if(pairMasterData != null)
                {
                    var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pairMasterData.Id);
                    if(pairStastics != null)
                    {
                        dataRes.Change24 = pairStastics.High24Hr - pairStastics.Low24Hr;
                        dataRes.ChangePer = pairStastics.ChangePer24;
                        dataRes.High24 = pairStastics.High24Hr;
                        dataRes.Low24 = pairStastics.Low24Hr;
                        dataRes.LastPrice = pairStastics.LTP;
                        dataRes.Volume24 = pairStastics.ChangeVol24;
                    }
                }
                return dataRes;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }


        public VolumeDataRespose GetVolumeDataByPair(long PairId)
        {
            decimal ChangePer = 0;
            decimal Volume24 = 0;

            VolumeDataRespose responsedata;
            try
            {
                responsedata = new VolumeDataRespose();
                var pairMasterData = _tradeMasterRepository.GetActiveById(PairId);

                if (pairMasterData != null)
                {
                    var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pairMasterData.Id);
                    var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pairMasterData.Id);
                    //GetPairAdditionalVal(pairMasterData.Id, pairDetailData.Currentrate, ref Volume24, ref ChangePer);

                    responsedata.PairId = pairMasterData.Id;
                    responsedata.PairName = pairMasterData.PairName;
                    responsedata.Currentrate = pairStastics.CurrentRate;
                    //volumedata.ChangePer = System.Math.Round(Volume24, 2);
                    //volumedata.Volume24 = System.Math.Round(ChangePer, 2);
                    responsedata.ChangePer = pairStastics.ChangePer24;
                    responsedata.Volume24 = pairStastics.ChangeVol24;
                    responsedata.High24Hr = pairStastics.High24Hr;
                    responsedata.Low24Hr = pairStastics.Low24Hr;
                    responsedata.HighWeek = pairStastics.HighWeek;
                    responsedata.LowWeek = pairStastics.LowWeek;
                    responsedata.High52Week = pairStastics.High52Week;
                    responsedata.Low52Week = pairStastics.Low52Week;
                    responsedata.UpDownBit = pairStastics.UpDownBit;

                    return responsedata;
                }
                else
                {
                    return responsedata;
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public bool addSetteledTradeTransaction(SettledTradeTransactionQueue queueData)
        {
            try
            {
                var model = _settelTradeTranQueue.Add(queueData);
                if (model.Id != 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public PairRatesResponse GetPairRates(long PairId)
        {
            try
            {
                PairRatesResponse responseData = new PairRatesResponse();

                responseData = _frontTrnRepository.GetPairRates(PairId);
               
                return responseData;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int AddToFavouritePair(long PairId, long UserId)
        {
            try
            {
                var pairData = _tradeMasterRepository.GetById(PairId);
                if(pairData == null)
                {
                    return 2;
                }
                var favouritePair = _favouritePairRepository.GetSingle(x => x.PairId == PairId && x.UserId == UserId);
                if(favouritePair == null)
                {
                    //Add With First Time
                    favouritePair = new FavouritePair()
                    {
                        PairId = PairId,
                        UserId = UserId,
                        Status = 1,
                        CreatedBy = UserId,
                        CreatedDate = _basePage.UTC_To_IST()
                    };
                    favouritePair = _favouritePairRepository.Add(favouritePair);
                }
                else if(favouritePair != null)
                {
                    if(favouritePair.Status == 1)
                    {
                        return 1;  // already added as favourite pair
                    }
                    else if(favouritePair.Status == 9)
                    {
                        favouritePair.Status = 1;
                        _favouritePairRepository.Update(favouritePair);
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public int RemoveFromFavouritePair(long PairId, long UserId)
        {
            try
            {
                var favouritePair = _favouritePairRepository.GetSingle(x => x.PairId == PairId && x.UserId == UserId);
                if (favouritePair == null)
                {
                    return 1;
                }
                else if (favouritePair != null)
                {
                    favouritePair.Status = 9;
                    _favouritePairRepository.Update(favouritePair);
                }
                return 0;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        public List<FavouritePairInfo> GetFavouritePair(long UserId)
        {
            List<FavouritePairInfo> responsedata = new List<FavouritePairInfo>();
            try
            {
                var favouritepair = _favouritePairRepository.FindBy(x => x.UserId == UserId && x.Status == 1);
                if(favouritepair != null)
                {
                    foreach (var favPair in favouritepair)
                    {
                        FavouritePairInfo response = new FavouritePairInfo();
                        var pairMasterData = _tradeMasterRepository.GetById(favPair.PairId);
                        var pairDetailData = _tradeDetailRepository.GetSingle(x => x.PairId == pairMasterData.Id);
                        var chidService = _serviceMasterRepository.GetSingle(x => x.Id == pairMasterData.SecondaryCurrencyId);
                        var baseService = _serviceMasterRepository.GetSingle(x => x.Id == pairMasterData.BaseCurrencyId);
                        var pairStastics = _tradePairStastics.GetSingle(x => x.PairId == pairMasterData.Id);

                        response.PairId = pairMasterData.Id;
                        response.Pairname = pairMasterData.PairName;
                        response.Currentrate = pairStastics.CurrentRate;
                        response.BuyFees = pairDetailData.BuyFees;
                        response.SellFees = pairDetailData.SellFees;
                        response.ChildCurrency = chidService.Name;
                        response.Abbrevation = chidService.SMSCode;
                        response.BaseCurrency = baseService.Name;
                        response.BaseAbbrevation = baseService.SMSCode;
                        response.ChangePer = pairStastics.ChangePer24;
                        response.Volume = pairStastics.ChangeVol24;
                        response.High24Hr = pairStastics.High24Hr;
                        response.Low24Hr = pairStastics.Low24Hr;
                        response.HighWeek = pairStastics.HighWeek;
                        response.LowWeek = pairStastics.LowWeek;
                        response.High52Week = pairStastics.High52Week;
                        response.Low52Week = pairStastics.Low52Week;
                        response.UpDownBit = pairStastics.UpDownBit;

                        responsedata.Add(response);
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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        #endregion

        #region parameterValidation
        public bool IsValidPairName(string Pair)
        {
            try
            {
                String Pattern = "^[A-Z_]{6,9}$";
                if(Regex.IsMatch(Pair, Pattern, RegexOptions.IgnoreCase))
                      return true;

                return false;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public Int16  IsValidTradeType(string Type)
        {
            //enTrnType
            try
            {
                if (Type.ToUpper().Equals("BUY"))
                    return Convert.ToInt16(enTrnType.Buy_Trade);
                else if (Type.ToUpper().Equals("SELL"))
                    return Convert.ToInt16(enTrnType.Sell_Trade);
                else
                    return 999;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public Int16  IsValidMarketType(string Type)
        {
            //enTransactionMarketType
            try
            {
                if (Type.ToUpper().Equals("LIMIT"))
                    return Convert.ToInt16(enTransactionMarketType.LIMIT);
                else if (Type.ToUpper().Equals("MARKET"))
                    return Convert.ToInt16(enTransactionMarketType.MARKET);
                else if (Type.ToUpper().Equals("STOP_LOSS"))
                    return Convert.ToInt16(enTransactionMarketType.STOP_Limit);
                else if (Type.ToUpper().Equals("SPOT"))
                    return Convert.ToInt16(enTransactionMarketType.SPOT);
                else
                    return 999;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public short IsValidStatus(string status)
        {
            try
            {
                if (status.ToUpper().Equals("SETTLED"))
                    return Convert.ToInt16(enTransactionStatus.Success);
                if (status.ToUpper().Equals("CURRENT"))
                    return Convert.ToInt16(enTransactionStatus.Hold);
                else
                    return 999;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool IsValidDateFormate(string date)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public void GetIntervalTimeValue(string Interval, ref int IntervalTime, ref string IntervalData)
        {
            try
            {
                if(Interval.Equals("1m"))
                {
                    IntervalTime = 1;
                    IntervalData = "MINUTE";
                }
                else if(Interval.Equals("3m"))
                {
                    IntervalTime = 3;
                    IntervalData = "MINUTE";
                }
                else if (Interval.Equals("5m"))
                {
                    IntervalTime = 5;
                    IntervalData = "MINUTE";
                }
                else if (Interval.Equals("15m"))
                {
                    IntervalTime = 15;
                    IntervalData = "MINUTE";
                }
                else if (Interval.Equals("30m"))
                {
                    IntervalTime = 30;
                    IntervalData = "MINUTE";
                }
                else if (Interval.Equals("1H"))
                {
                    IntervalTime = 1;
                    IntervalData = "HOUR";
                }
                else if (Interval.Equals("2H"))
                {
                    IntervalTime = 2;
                    IntervalData = "HOUR";
                }
                else if (Interval.Equals("4H"))
                {
                    IntervalTime = 4;
                    IntervalData = "HOUR";
                }
                else if (Interval.Equals("6H"))
                {
                    IntervalTime = 6;
                    IntervalData = "HOUR";
                }
                else if (Interval.Equals("12H"))
                {
                    IntervalTime = 12;
                    IntervalData = "HOUR";
                }
                else if (Interval.Equals("1D"))
                {
                    IntervalTime = 1;
                    IntervalData = "DAY";
                }
                else if (Interval.Equals("1W"))
                {
                    IntervalTime = 1;
                    IntervalData = "WEEK";
                }
                else if (Interval.Equals("1M"))
                {
                    IntervalTime = 1;
                    IntervalData = "MONTH";
                }
                else
                {
                    IntervalTime = 0;
                }

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
       
        #endregion
    }
}
