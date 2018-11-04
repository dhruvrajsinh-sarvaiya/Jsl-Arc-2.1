using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    public class CommonWalletFunction : ICommonWalletFunction
    {
        private readonly ICommonRepository<WalletMaster> _commonRepository;
        private readonly ICommonRepository<MemberShadowBalance> _ShadowBalRepo;
        private readonly IWalletRepository _walletRepository1;
        private readonly IWalletRepository _repository;
        private readonly ICommonRepository<MemberShadowLimit> _ShadowLimitRepo;
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        private readonly ICommonRepository<ChargeRuleMaster> _chargeRuleMaster;
        private readonly ICommonRepository<LimitRuleMaster> _limitRuleMaster;

        public CommonWalletFunction(ICommonRepository<WalletMaster> commonRepository, IWalletRepository repository, ICommonRepository<MemberShadowBalance> ShadowBalRepo, IWalletRepository walletRepository, ICommonRepository<MemberShadowLimit> ShadowLimitRepo, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ICommonRepository<ChargeRuleMaster> chargeRuleMaster, ICommonRepository<LimitRuleMaster> limitRuleMaster)
        {
            _commonRepository = commonRepository;
            _repository = repository;
            _ShadowBalRepo = ShadowBalRepo;
            _walletRepository1 = walletRepository;
            _ShadowLimitRepo = ShadowLimitRepo;
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
            _limitRuleMaster = limitRuleMaster;
            _chargeRuleMaster = chargeRuleMaster;
        }

        public decimal GetLedgerLastPostBal(long walletId)
        {
            try
            {
                var bal =_repository.GetLedgerLastPostBal(walletId);
                return 0;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public enErrorCode CheckShadowLimit(long WalletID, decimal Amount)
        {
            try
            {
                var Walletobj = _commonRepository.GetSingle(item => item.Id == WalletID);
                if (Walletobj != null)
                {
                    var Balobj = _ShadowBalRepo.GetSingle(item => item.WalletID == WalletID);
                    if (Balobj != null)
                    {
                        if ((Balobj.ShadowAmount + Amount) <= Walletobj.Balance)
                        {
                            return enErrorCode.Success;
                        }
                        return enErrorCode.InsufficientBalance;
                    }
                    else
                    {
                        var typeobj = _walletRepository1.GetTypeMappingObj(Walletobj.UserID);
                        if (typeobj != -1) //ntrivedi 04-11-2018 
                        {
                            var Limitobj = _ShadowLimitRepo.GetSingle(item => item.MemberTypeId == typeobj);
                            if (Limitobj != null)
                            {
                                if ((Limitobj.ShadowLimitAmount + Amount) <= Walletobj.Balance)
                                {
                                    return enErrorCode.Success;
                                }
                                return enErrorCode.InsufficientBalance;
                            }
                            return enErrorCode.Success; // IF ENTRY NOT FOUND THEN SUCCESS NTRIVEDI
                        }
                        return enErrorCode.MemberTypeNotFound;
                    }
                }
                return enErrorCode.WalletNotFound;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                throw ex;
            }
        }

        public ServiceLimitChargeValue GetServiceLimitChargeValue(enTrnType TrnType, string CoinName)
        {
            try
            {
                ServiceLimitChargeValue response;
                var walletType = _WalletTypeMasterRepository.GetSingle(x => x.WalletTypeName == CoinName);
                if (walletType != null)
                {
                    response = new ServiceLimitChargeValue();
                    var limitData = _limitRuleMaster.GetSingle(x => x.TrnType == TrnType && x.WalletType == walletType.Id);
                    var chargeData = _chargeRuleMaster.GetSingle(x => x.TrnType == TrnType && x.WalletType == walletType.Id);

                    if (limitData != null && chargeData != null)
                    {
                        response.CoinName = walletType.WalletTypeName;
                        response.TrnType = limitData.TrnType;
                        response.MinAmount = limitData.MinAmount;
                        response.MaxAmount = limitData.MaxAmount;
                        response.ChargeType = chargeData.ChargeType;
                        response.ChargeValue = chargeData.ChargeValue;
                    }
                    return response;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

    }
}
