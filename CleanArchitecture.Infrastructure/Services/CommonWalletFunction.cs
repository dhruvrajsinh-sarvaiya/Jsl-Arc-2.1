using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services
{
    public class CommonWalletFunction
    {
        private readonly ICommonRepository<WalletLedger> _commonRepository;
        private readonly IWalletRepository _repository;

        public CommonWalletFunction(ICommonRepository<WalletLedger> commonRepository, IWalletRepository repository)
        {
            _commonRepository = commonRepository;
            _repository = repository;
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
    }
}
