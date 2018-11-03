using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
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


        public CommonWalletFunction(ICommonRepository<WalletMaster> commonRepository, IWalletRepository repository, ICommonRepository<MemberShadowBalance> ShadowBalRepo, IWalletRepository walletRepository, ICommonRepository<MemberShadowLimit> ShadowLimitRepo)
        {
            _commonRepository = commonRepository;
            _repository = repository;
            _ShadowBalRepo = ShadowBalRepo;
            _walletRepository1 = walletRepository;
            _ShadowLimitRepo = ShadowLimitRepo;
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
                        if (typeobj != 0)
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
                            return enErrorCode.NotFoundLimit;
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

    }
}
