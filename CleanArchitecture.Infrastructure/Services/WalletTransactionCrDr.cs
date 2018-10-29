using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Infrastructure.DTOClasses;
using System.Threading.Tasks;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.Entities.Wallet;
using System.Linq;
using CleanArchitecture.Core.ViewModels.WalletConfiguration;
using System.Collections;

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletTransactionCrDr : BasePage , IWalletTransactionCrDr
    {
        private readonly ILogger<WalletService> _log;
        private readonly ICommonRepository<WalletMaster> _commonRepository;
        //private readonly ICommonRepository<WalletLimitConfiguration> _LimitcommonRepository;
       // private readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyCommonRepository;
        private readonly ICommonRepository<WalletOrder> _walletOrderRepository;
        private readonly ICommonRepository<AddressMaster> _addressMstRepository;
        private readonly ICommonRepository<TrnAcBatch> _trnBatch;
        //private readonly ICommonRepository<TradeBitGoDelayAddresses> _bitgoDelayRepository;
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepo;
        //private readonly ICommonRepository<BeneficiaryMaster> _BeneficiarycommonRepository;
        //private readonly ICommonRepository<UserPreferencesMaster> _UserPreferencescommonRepository;

        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        private readonly IWalletRepository _walletRepository1;
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        private readonly IWalletService _walletService;
        //private readonly IWebApiRepository _webApiRepository;
        //private readonly IWebApiSendRequest _webApiSendRequest;
        //private readonly IGetWebRequest _getWebRequest;
        //private readonly WebApiParseResponse _WebApiParseResponse;


        public WalletTransactionCrDr(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository,
           ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,          
           IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
           ILogger<BasePage> logger, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository,
           ICommonRepository<WalletAllowTrn> WalletAllowTrnRepo, ICommonRepository<WalletLimitConfiguration> WalletLimitConfig,IWalletService walletService) : base(logger)
        {
            _log = log;
            _commonRepository = commonRepository;
            _walletOrderRepository = walletOrderRepository;
            //_walletRepository = repository;
            //_bitgoDelayRepository = bitgoDelayRepository;
            _trnBatch = BatchLogger;
            _walletRepository1 = walletRepository;
            //_webApiRepository = webApiRepository;
            //_webApiSendRequest = webApiSendRequest;
            //_thirdPartyCommonRepository = thirdpartyCommonRepo;
            //_getWebRequest = getWebRequest;
            _addressMstRepository = addressMaster;
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
            //_WalletAllowTrnRepository = WalletAllowTrnRepository;
            //_WebApiParseResponse = WebApiParseResponse;
            //_walletLedgerRepository = walletledgerrepo;
            _WalletAllowTrnRepo = WalletAllowTrnRepo;
            _walletService = walletService;
            //_LimitcommonRepository = WalletLimitConfig;
            //_BeneficiarycommonRepository = BeneficiaryMasterRepo;
            //_UserPreferencescommonRepository = UserPreferenceRepo;
        }

        public WalletDrCrResponse InsertWalletTQDebit(string timestamp, long walletID, string coinName, decimal amount, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType, enWalletTranxOrderType enWalletTranx, enWalletLimitType enWalletLimit)
        {
            try
            {
                WalletMaster Walletobj;
                string remarks = "";
                WalletTypeMaster walletTypeMaster;
                WalletTransactionQueue objTQ;
                //long walletTypeID;
                WalletDrCrResponse resp = new WalletDrCrResponse();
                long owalletID, orgID;

                walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                if (walletTypeMaster == null)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName };
                }
                Walletobj = _commonRepository.GetById(walletID);
                //orgID = _walletRepository1.getOrgID();
                //if (orgID == 0)
                //{
                //    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OrgIDNotFound, ErrorCode = enErrorCode.OrgIDNotFound };
                //}
                //Walletobj = _commonRepository.FindBy(e => e.WalletTypeID == Walletobj.WalletTypeID && e.UserID == orgID && e.IsDefaultWallet == 1 && e.Status == 1).FirstOrDefault();
                //if (Walletobj == null)
                //{
                //    //tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid().ToString(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 2, EnResponseMessage.InvalidWallet);
                //    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                //}
                if (Walletobj.Status != 1 || Walletobj.IsValid == false)
                {
                    // insert with status=2 system failed
                    objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (TrnRefNo == 0) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (amount <= 0)
                {
                    // insert with status=2 system failed
                    objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (Walletobj.Balance < amount)
                {
                    // insert with status=2 system failed
                    objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InsufficantBal, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficantBal, ErrorCode = enErrorCode.InsufficantBal, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                int count = _walletService.CheckTrnRefNo(TrnRefNo, enWalletTranx, trnType);
                if (count != 0)
                {
                    // insert with status=2 system failed
                    objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.AlredyExist, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                // ntrivedi need to add condition for allowd wallet trntype 
                // ntrivedi need to match transactionaccount
                // ntrivedi need to check limit (deposit , withdrawal , trading ) set by user 
                objTQ = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, 0, "Inserted", trnType);
                objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public WalletDrCrResponse DepositionWalletOperation(string timestamp, string address, string coinName, decimal amount, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType, enWalletTranxOrderType enWalletTranx, enWalletLimitType enWalletLimit)
        {
            try
            {
                WalletMaster dWalletobj, cWalletObj;
                string DrRemarks = "", CrRemarks = "";
                WalletTypeMaster walletTypeMaster;
                WalletTransactionQueue objTQDr, objTQCr;
                //long walletTypeID;
                WalletDrCrResponse resp = new WalletDrCrResponse();
                long owalletID, orgID;
                WalletTransactionOrder woObj;


                // moved inside InsertWalletTQDebit
                //walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                //if (walletTypeMaster == null)
                //{
                //    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName };
                //}
                owalletID = _walletService.GetWalletByAddress(address);
                if (owalletID == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAddress, ErrorCode = enErrorCode.InvalidAddress };
                }
                cWalletObj = _commonRepository.GetById(owalletID);
                orgID = _walletRepository1.getOrgID();
                if (orgID == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OrgIDNotFound, ErrorCode = enErrorCode.OrgIDNotFound };
                }
                dWalletobj = _commonRepository.FindBy(e => e.WalletTypeID == cWalletObj.WalletTypeID && e.UserID == orgID && e.IsDefaultWallet == 1 && e.Status == 1).FirstOrDefault();
                if (dWalletobj == null)
                {
                    //tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid().ToString(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 2, EnResponseMessage.InvalidWallet);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                }

                resp = InsertWalletTQDebit(timestamp, dWalletobj.Id, coinName, amount, TrnRefNo, serviceType, trnType, enWalletTranx, enWalletLimit);
                if (resp.Status != 0 && resp.TrnNo == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = resp.StatusMsg, ErrorCode = resp.ErrorCode };
                }
                if (cWalletObj.Status != 1 || cWalletObj.IsValid == false)
                {
                    // insert with status=2 system failed
                    objTQCr = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Credit, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, dWalletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQCr = _walletRepository1.AddIntoWalletTransactionQueue(objTQCr, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQCr.TrnNo, Status = objTQCr.Status, StatusMsg = objTQCr.StatusMsg };
                }

                objTQDr = _walletRepository1.GetTransactionQueue(resp.TrnNo);
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                DrRemarks = "Debit for Deposition TrnNo:" + TrnRefNo;
                WalletLedger walletLedgerDr = _walletService.GetWalletLedgerObj(dWalletobj.Id, 0, amount, 0, trnType, serviceType, objTQDr.TrnNo, DrRemarks, dWalletobj.Balance, 1);
                TransactionAccount tranxAccounDrt = _walletService.GetTransactionAccount(dWalletobj.Id, 1, batchObj.Id, amount, 0, objTQDr.TrnNo, DrRemarks, 1);
                dWalletobj.DebitBalance(amount);
                objTQDr.Status = enTransactionStatus.Success;
                objTQDr.StatusMsg = "Success";
                DrRemarks = "Credit for Deposition TrnNo:" + TrnRefNo;
                objTQCr = _walletService.InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Credit, amount, TrnRefNo, UTC_To_IST(), null, cWalletObj.Id, coinName, cWalletObj.UserID, timestamp, 0, "Inserted", trnType);
                objTQCr = _walletRepository1.AddIntoWalletTransactionQueue(objTQCr, 1);
                woObj = _walletService.InsertIntoWalletTransactionOrder(null, UTC_To_IST(), cWalletObj.Id, dWalletobj.Id, amount, coinName, objTQCr.TrnNo, objTQDr.TrnNo, 0, "Inserted");
                woObj = _walletRepository1.AddIntoWalletTransactionOrder(woObj, 1);
                WalletLedger walletLedgerCr = _walletService.GetWalletLedgerObj(cWalletObj.Id, 0, 0, amount, trnType, serviceType, objTQCr.TrnNo, DrRemarks, cWalletObj.Balance, 1);
                TransactionAccount tranxAccountCr = _walletService.GetTransactionAccount(cWalletObj.Id, 1, batchObj.Id, 0, amount, objTQCr.TrnNo, DrRemarks, 1);
                cWalletObj.CreditBalance(amount);
                //var objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, 1, "Updated");
                objTQCr.Status = enTransactionStatus.Success;
                objTQCr.StatusMsg = "Success";
                objTQCr.UpdatedDate = UTC_To_IST();

                _walletRepository1.WalletCreditDebitwithTQ(walletLedgerCr, walletLedgerDr, tranxAccountCr, tranxAccounDrt, dWalletobj, cWalletObj, objTQCr, objTQDr, woObj);
                //CreditWalletDrArryTrnID[] arryTrnID = new CreditWalletDrArryTrnID [1];
                //arryTrnID[0].Amount = amount;
                //arryTrnID[0].DrTrnRefNo = TrnRefNo;

                //resp = GetWalletCreditNew(coinName, timestamp, trnType, amount, cWalletObj.UserID, cWalletObj.AccWalletID,  arryTrnID, TrnRefNo,1,enWalletTranxOrderType.Credit,enServiceType.WalletService );
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = resp.TrnNo, Status = resp.Status, StatusMsg = resp.StatusMsg };

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

    }
}
