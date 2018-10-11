using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Interfaces.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Services.Configuration
{
    public class WalletConfigurationService: IWalletConfigurationService
    {
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        public WalletConfigurationService(ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository)
        {
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
        }

        public List<WalletTypeMaster> ListAllWalletTypeMaster()
        {
            List<WalletTypeMaster> coin = new List<WalletTypeMaster>();
            coin = _WalletTypeMasterRepository.List();
            return coin;
        }
    }
}
