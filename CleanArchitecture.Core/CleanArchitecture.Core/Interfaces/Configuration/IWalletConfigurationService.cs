using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces.Configuration
{
    public interface IWalletConfigurationService
    {
        //vsolanki 11-10-2018
        #region "wallettypemaster"
        ListWalletTypeMasterResponse ListAllWalletTypeMaster();

        WalletTypeMasterResponse AddWalletTypeMaster(WalletTypeMasterRequest addWalletTypeMasterRequest, long Userid);

        WalletTypeMasterResponse UpdateWalletTypeMaster(WalletTypeMasterRequest updateWalletTypeMasterRequest, long Userid, long WalletTypeId);

        BizResponseClass DisableWalletTypeMaster(long WalletTypeId);

        WalletTypeMasterResponse GetWalletTypeMasterById(long WalletTypeId);
        #endregion
    }
}
