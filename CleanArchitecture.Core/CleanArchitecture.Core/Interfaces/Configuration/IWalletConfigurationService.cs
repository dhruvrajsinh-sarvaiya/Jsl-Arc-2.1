using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Configuration
{
    public interface IWalletConfigurationService
    {
        List<WalletTypeMaster> ListAllWalletTypeMaster();

        WalletTypeMaster AddWalletTypeMaster(WalletTypeMasterRequest addWalletTypeMasterRequest, long Userid);

        WalletTypeMaster UpdateWalletTypeMaster(WalletTypeMasterRequest updateWalletTypeMasterRequest, long Userid, long WalletTypeId);

    }
}
