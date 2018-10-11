using CleanArchitecture.Core.ApiModels;
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
        #region "DI"

        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        private readonly ILogger<WalletConfigurationService> _log;

        #endregion

        #region "cotr"
        public WalletConfigurationService(ILogger<WalletConfigurationService> log, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ILogger<BasePage> logger) : base(logger)
        {
            _log = log;
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
        }
        #endregion

        #region "Methods"

        #region "WalletTypeMaster"

        //vsolanki 11-10-2018 List the wallettypemaster
        public ListWalletTypeMasterResponse ListAllWalletTypeMaster()
        {
            ListWalletTypeMasterResponse listWalletTypeMasterResponse = new ListWalletTypeMasterResponse();
            try
            {
                IEnumerable<WalletTypeMaster> coin = new List<WalletTypeMaster>();
                coin = _WalletTypeMasterRepository.FindBy(item => item.Status != Convert.ToInt16(ServiceStatus.Disable));
                if(coin==null)
                {
                    listWalletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    listWalletTypeMasterResponse.ReturnMsg = EnResponseMessage.NotFound;
                }
                else
                {
                    listWalletTypeMasterResponse.walletTypeMasters = coin;
                    listWalletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    listWalletTypeMasterResponse.ReturnMsg = EnResponseMessage.FindRecored;
                }
                           
                return listWalletTypeMasterResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                listWalletTypeMasterResponse.ReturnCode = enResponseCode.InternalError;
                return listWalletTypeMasterResponse;
            }
        }

        //vsolanki 11-10-2018 insert into wallettypemaster
        public WalletTypeMasterResponse AddWalletTypeMaster(WalletTypeMasterRequest addWalletTypeMasterRequest, long Userid)
        {
            WalletTypeMasterResponse walletTypeMasterResponse = new WalletTypeMasterResponse();
            WalletTypeMaster _walletTypeMaster = new WalletTypeMaster();
            try
            {
                if(addWalletTypeMasterRequest==null)
                {
                    walletTypeMasterResponse.ReturnCode= enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.NotFound;
                    return walletTypeMasterResponse;
                }
                else
                { 
                _walletTypeMaster.CreatedBy = Userid;
                _walletTypeMaster.CreatedDate = UTC_To_IST();
                _walletTypeMaster.IsDepositionAllow = addWalletTypeMasterRequest.IsDepositionAllow;
                _walletTypeMaster.IsTransactionWallet = addWalletTypeMasterRequest.IsTransactionWallet;
                _walletTypeMaster.IsWithdrawalAllow = addWalletTypeMasterRequest.IsWithdrawalAllow;
                _walletTypeMaster.WalletTypeName = addWalletTypeMasterRequest.WalletTypeName;
                _walletTypeMaster.Discription = addWalletTypeMasterRequest.Discription;
                _walletTypeMaster.Status = Convert.ToInt16(ServiceStatus.Active);
                _WalletTypeMasterRepository.Add(_walletTypeMaster);
                    walletTypeMasterResponse.walletTypeMaster = _walletTypeMaster;
                    walletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.RecordAdded;
                    return walletTypeMasterResponse;
                }
                //return _walletTypeMaster;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                walletTypeMasterResponse.ReturnCode = enResponseCode.InternalError;
                return walletTypeMasterResponse;
            }
        }

        //vsolanki 11-10-2018 upadte into wallettypemaster
        public WalletTypeMasterResponse UpdateWalletTypeMaster(WalletTypeMasterRequest updateWalletTypeMasterRequest, long Userid, long WalletTypeId)
        {
            WalletTypeMasterResponse walletTypeMasterResponse = new WalletTypeMasterResponse();
            try
            {
                // WalletTypeMaster _walletTypeMaster = new WalletTypeMaster();
                var _walletTypeMaster = _WalletTypeMasterRepository.GetById(WalletTypeId);
                if (_walletTypeMaster == null)
                {
                    walletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.NotFound;
                    return walletTypeMasterResponse;
                }
                else
                {
                    _walletTypeMaster.UpdatedBy = Userid;
                    _walletTypeMaster.UpdatedDate = UTC_To_IST();

                    _walletTypeMaster.IsDepositionAllow = updateWalletTypeMasterRequest.IsDepositionAllow;
                    _walletTypeMaster.IsTransactionWallet = updateWalletTypeMasterRequest.IsTransactionWallet;
                    _walletTypeMaster.IsWithdrawalAllow = updateWalletTypeMasterRequest.IsWithdrawalAllow;
                    _walletTypeMaster.WalletTypeName = updateWalletTypeMasterRequest.WalletTypeName;
                    _walletTypeMaster.Discription = updateWalletTypeMasterRequest.Discription;
                    _walletTypeMaster.Status = updateWalletTypeMasterRequest.Status;

                    _WalletTypeMasterRepository.Update(_walletTypeMaster);
                    walletTypeMasterResponse.walletTypeMaster = _walletTypeMaster;
                    walletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.RecordUpdated;
                    return walletTypeMasterResponse;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                walletTypeMasterResponse.ReturnCode = enResponseCode.InternalError;
                return walletTypeMasterResponse;
            }
        }

        //vsolanki 11-10-2018 delete from wallettypemaster
        public BizResponseClass DisableWalletTypeMaster(long WalletTypeId)
        {
            try
            {
                var _walletTypeMaster = _WalletTypeMasterRepository.GetById(WalletTypeId);
                if(_walletTypeMaster==null)
                {
                    return new BizResponseClass { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }         
                else
                {
                    //_WalletTypeMasterRepository.Delete(_walletTypeMaster);
                    _walletTypeMaster.DisableStatus();
                    _WalletTypeMasterRepository.Update(_walletTypeMaster);
                    return new BizResponseClass {  ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordDisable };
                }            
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                return new BizResponseClass { ReturnCode = enResponseCode.InternalError,   };
            }
        }

        //vsolanki 11-10-2018 wallettypemaster Get by id
        public WalletTypeMasterResponse GetWalletTypeMasterById(long WalletTypeId)
        {
            WalletTypeMasterResponse walletTypeMasterResponse = new WalletTypeMasterResponse();
            try
            {
                var _walletTypeMaster = _WalletTypeMasterRepository.GetSingle(item => item.Id== WalletTypeId && item.Status != Convert.ToInt16(ServiceStatus.Disable));
                if(_walletTypeMaster==null)
                {
                    walletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.NotFound;
                    return walletTypeMasterResponse;
                }
                else
                {
                    walletTypeMasterResponse.walletTypeMaster = _walletTypeMaster;
                    walletTypeMasterResponse.ReturnCode = enResponseCode.Success;
                    walletTypeMasterResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    return walletTypeMasterResponse;
                }
               //return _walletTypeMaster;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                walletTypeMasterResponse.ReturnCode = enResponseCode.InternalError;
                return walletTypeMasterResponse;
            }
        }
        #endregion

        #endregion
    }
}
