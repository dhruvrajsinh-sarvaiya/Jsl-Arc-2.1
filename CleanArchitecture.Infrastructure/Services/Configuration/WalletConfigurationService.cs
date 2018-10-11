using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Configuration;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class WalletConfigurationService : BasePage, IWalletConfigurationService
    {
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        readonly ILogger<WalletConfigurationService> _log;

        public WalletConfigurationService(ILogger<WalletConfigurationService> log, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ILogger<BasePage> logger) : base(logger)
        {
            _log = log;
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
        }

        public List<WalletTypeMaster> ListAllWalletTypeMaster()
        {
            try
            {
                List<WalletTypeMaster> coin = new List<WalletTypeMaster>();
                coin = _WalletTypeMasterRepository.List();
                return coin;
            }
           catch(Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public WalletTypeMaster AddWalletTypeMaster(AddWalletTypeMasterRequest addWalletTypeMasterRequest, long Userid)
        {
            try
            {
                WalletTypeMaster _walletTypeMaster = new WalletTypeMaster();
                _walletTypeMaster.CreatedBy = Userid;
                _walletTypeMaster.CreatedDate = UTC_To_IST();
                _walletTypeMaster.IsDepositionAllow = addWalletTypeMasterRequest.IsDepositionAllow;
                _walletTypeMaster.IsTransactionWallet = addWalletTypeMasterRequest.IsTransactionWallet;
                _walletTypeMaster.IsWithdrawalAllow = addWalletTypeMasterRequest.IsWithdrawalAllow;
                _walletTypeMaster.WalletTypeName = addWalletTypeMasterRequest.WalletTypeName;
                _walletTypeMaster.Discription = addWalletTypeMasterRequest.Discription;
                _walletTypeMaster.Status = Convert.ToInt16(ServiceStatus.Active);
                _WalletTypeMasterRepository.Add(_walletTypeMaster);
                return _walletTypeMaster;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

    }
}
