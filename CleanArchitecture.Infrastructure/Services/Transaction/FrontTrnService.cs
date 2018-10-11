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

namespace CleanArchitecture.Infrastructure.Services.Transaction
{
    public class FrontTrnService : IFrontTrnService
    {
        private readonly IFrontTrnRepository _frontTrnRepository;
        private readonly ICommonRepository<TradePairMaster> _tradeMasterRepository;
        private readonly ICommonRepository<TradePairDetail> _tradeDetailRepository;
        private readonly ICommonRepository<ServiceMaster> _serviceMasterRepository;
        private readonly ILogger<FrontTrnService> _logger;

        public FrontTrnService(IFrontTrnRepository frontTrnRepository, ICommonRepository<TradePairMaster> tradeMasterRepository, ICommonRepository<TradePairDetail> tradeDetailRepository, ILogger<FrontTrnService> logger, ICommonRepository<ServiceMaster> serviceMasterRepository)
        {
            _frontTrnRepository = frontTrnRepository;
            _tradeMasterRepository = tradeMasterRepository;
            _tradeDetailRepository = tradeDetailRepository;
            _logger = logger;
            _serviceMasterRepository = serviceMasterRepository;
        }
        public List<GetActiveOrderInfo> GetActiveOrder(long MemberID)
        {
            try
            {
                List<ActiveOrderDataResponse> ActiveOrderList = _frontTrnRepository.GetActiveOrder(MemberID);
                var response = ActiveOrderList.ConvertAll(x => new GetActiveOrderInfo { order_id = x.Id, pair_name = x.Order_Currency, price = x.Price, side = x.Type });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public List<BasePairResponse> GetTradePairAsset()
        {
            List<BasePairResponse> responsedata;
            try
            {
                responsedata = new List<BasePairResponse>();
                var basePairData = _tradeMasterRepository.GetAll().GroupBy(x => x.BaseCurrencyId).Select(x => x.FirstOrDefault());

                if(basePairData != null)
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

                            tradePair.PairId = pmdata.Id;
                            tradePair.Pairname = pmdata.PairName;
                            tradePair.Currentrate = pairDetailData.Currentrate;
                            tradePair.Volume = pairDetailData.Volume;
                            tradePair.Fee = pairDetailData.Fee;
                            tradePair.ChildCurrency = chidService.Name;
                            tradePair.Abbrevation = chidService.SMSCode;
                            tradePair.ChangePer = 0;

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
                        volumedata.PairId = pmdata.Id;
                        volumedata.Currentrate = pairDetailData.Currentrate;
                        volumedata.ChangePer = 0;
                        volumedata.Volume24 = 0;

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
            throw new NotImplementedException();
        }
    }
}
