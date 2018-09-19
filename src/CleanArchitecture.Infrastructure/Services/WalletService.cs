using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Entities;

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : IWalletService
    {
        readonly ILogger<WalletService> _log;
        readonly IWalletRepository<WalletMaster> _walletRepository;


        public WalletService(ILogger<WalletService> log, IWalletRepository<WalletMaster> repository)
        {
            _log = log;
            _walletRepository = repository;
        }

        public bool CreditWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }

        public bool DebitWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }
        
        public decimal GetUserBalance(int walletId)
        {
            try
            {
                var obj = _walletRepository.GetById(walletId);
                return obj.Balance;                
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool WalletBalanceCheck(decimal amount,long walletid)
        {
            try
            {
                var obj =_walletRepository.GetById(walletid);
                if(obj.Balance < amount)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
    }
}
