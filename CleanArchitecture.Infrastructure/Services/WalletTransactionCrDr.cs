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
using CleanArchitecture.Core.Helpers;


namespace CleanArchitecture.Infrastructure.Services
{
    public class WalletTransactionCrDr : BasePage, IWalletTransactionCrDr
    {
        // private readonly ILogger<WalletService> _log;
        private readonly ISignalRService _signalRService;
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
        private readonly ICommonRepository<TransactionAccount> _TransactionAccountsRepository;
       // private readonly IWalletService _walletService;

        private readonly ICommonWalletFunction _commonWalletFunction;
        private readonly ICommonRepository<ChargeRuleMaster> _chargeRuleMaster;
        private readonly ICommonRepository<LimitRuleMaster> _limitRuleMaster;
        //private readonly IWebApiRepository _webApiRepository;
        //private readonly IWebApiSendRequest _webApiSendRequest;
        //private readonly IGetWebRequest _getWebRequest;
        //private readonly WebApiParseResponse _WebApiParseResponse;


        public WalletTransactionCrDr(ICommonRepository<WalletMaster> commonRepository,
           ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,
           IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
           ILogger<BasePage> logger, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository,
           ICommonRepository<WalletAllowTrn> WalletAllowTrnRepo, ICommonRepository<WalletLimitConfiguration> WalletLimitConfig, ICommonRepository<TransactionAccount> TransactionAccountsRepository, ICommonWalletFunction commonWalletFunction,
           ICommonRepository<ChargeRuleMaster> chargeRuleMaster, ICommonRepository<LimitRuleMaster> limitRuleMaster, ISignalRService signalRService) : base(logger)
        {
            //_walletService = walletService;
            // _log = log;
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
            // _walletService = walletService;
            //_LimitcommonRepository = WalletLimitConfig;
            //_BeneficiarycommonRepository = BeneficiaryMasterRepo;
            //_UserPreferencescommonRepository = UserPreferenceRepo;
            _TransactionAccountsRepository = TransactionAccountsRepository;
            _signalRService = signalRService;
            _commonWalletFunction = commonWalletFunction;
            _chargeRuleMaster = chargeRuleMaster;
            _limitRuleMaster = limitRuleMaster;
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
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }

                if (!CheckUserBalance(Walletobj.Id))
                {
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.BalMismatch, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.SettedBalanceMismatch, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (TrnRefNo == 0) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (amount <= 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (Walletobj.Balance < amount)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InsufficantBal, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficantBal, ErrorCode = enErrorCode.InsufficantBal, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                // ntrivedi 03-11-2018
                if (_commonWalletFunction.CheckShadowLimit(Walletobj.Id, amount) != enErrorCode.Success)
                {
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.ShadowLimitExceed, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ShadowLimitExceed, ErrorCode = enErrorCode.ShadowBalanceExceed, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                //vsolanki 208-11-1
                var charge = GetServiceLimitChargeValue(enTrnType.Deposit, coinName);//for deposit
                if (charge.MaxAmount < amount && charge.MinAmount > amount)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ProcessTrn_AmountBetweenMinMaxMsg, ErrorCode = enErrorCode.ProcessTrn_AmountBetweenMinMax };
                }

                int count = CheckTrnRefNo(TrnRefNo, enWalletTranx, trnType);
                if (count != 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.AlredyExist, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                // ntrivedi need to add condition for allowd wallet trntype 
                // ntrivedi need to match transactionaccount
                // ntrivedi need to check limit (deposit , withdrawal , trading ) set by user 
                objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Debit, amount, TrnRefNo, UTC_To_IST(), null, Walletobj.Id, coinName, Walletobj.UserID, timestamp, 0, "Inserted", trnType);
                objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };

            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public WalletDrCrResponse DepositionWalletOperation(string timestamp, string address, string coinName, decimal amount, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType, enWalletTranxOrderType enWalletTranx, enWalletLimitType enWalletLimit, enTrnType routeTrnType, string Token = "")
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
                owalletID = GetWalletByAddress(address);
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
                //ntrivedi 03-11-2018
                var charge = _commonWalletFunction.GetServiceLimitChargeValue(enTrnType.Deposit, coinName);
                if (charge.MaxAmount < amount && charge.MinAmount > amount)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ProcessTrn_AmountBetweenMinMaxMsg, ErrorCode = enErrorCode.ProcessTrn_AmountBetweenMinMax };
                }
                resp = InsertWalletTQDebit(timestamp, dWalletobj.Id, coinName, amount, TrnRefNo, serviceType, enWalletTrnType.Dr_Debit, enWalletTranx, enWalletLimit);
                if (resp.ReturnCode != 0 || resp.Status != enTransactionStatus.Initialize)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = resp.StatusMsg, ErrorCode = resp.ErrorCode };
                }
                if (cWalletObj.Status != 1 || cWalletObj.IsValid == false)
                {
                    // insert with status=2 system failed
                    objTQCr = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Credit, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, dWalletobj.UserID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQCr = _walletRepository1.AddIntoWalletTransactionQueue(objTQCr, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQCr.TrnNo, Status = objTQCr.Status, StatusMsg = objTQCr.StatusMsg };
                }

                objTQDr = _walletRepository1.GetTransactionQueue(resp.TrnNo);
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                DrRemarks = "Debit for Deposition TrnNo:" + TrnRefNo;
                WalletLedger walletLedgerDr = GetWalletLedgerObj(dWalletobj.Id, cWalletObj.Id, amount, 0, enWalletTrnType.Dr_Debit, serviceType, objTQDr.TrnNo, DrRemarks, dWalletobj.Balance, 1);
                TransactionAccount tranxAccounDrt = GetTransactionAccount(dWalletobj.Id, 1, batchObj.Id, amount, 0, objTQDr.TrnNo, DrRemarks, 1);
                dWalletobj.DebitBalance(amount);
                objTQDr.Status = enTransactionStatus.Success;
                objTQDr.StatusMsg = "Success";
                DrRemarks = "Credit for Deposition TrnNo:" + TrnRefNo;
                objTQCr = InsertIntoWalletTransactionQueue(Guid.NewGuid(), enWalletTranxOrderType.Credit, amount, TrnRefNo, UTC_To_IST(), null, cWalletObj.Id, coinName, cWalletObj.UserID, timestamp, 0, "Inserted", trnType);
                objTQCr = _walletRepository1.AddIntoWalletTransactionQueue(objTQCr, 1);
                woObj = InsertIntoWalletTransactionOrder(null, UTC_To_IST(), cWalletObj.Id, dWalletobj.Id, amount, coinName, objTQCr.TrnNo, objTQDr.TrnNo, 0, "Inserted");
                woObj = _walletRepository1.AddIntoWalletTransactionOrder(woObj, 1);
                WalletLedger walletLedgerCr = GetWalletLedgerObj(cWalletObj.Id, dWalletobj.Id, 0, amount, trnType, serviceType, objTQCr.TrnNo, DrRemarks, cWalletObj.Balance, 1);
                TransactionAccount tranxAccountCr = GetTransactionAccount(cWalletObj.Id, 1, batchObj.Id, 0, amount, objTQCr.TrnNo, DrRemarks, 1);
                cWalletObj.CreditBalance(amount);
                //var objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, 1, "Updated");
                objTQCr.Status = enTransactionStatus.Success;
                objTQCr.StatusMsg = "Success";
                objTQCr.UpdatedDate = UTC_To_IST();
                woObj.Status = enTransactionStatus.Success;
                woObj.StatusMsg = "Deposition success for RefNo :" + TrnRefNo;
                woObj.UpdatedDate = UTC_To_IST();
                objTQDr.SettedAmt = amount;
                _walletRepository1.WalletCreditDebitwithTQ(walletLedgerDr, walletLedgerCr, tranxAccountCr, tranxAccounDrt, dWalletobj, cWalletObj, objTQCr, objTQDr, woObj);
                //ntrivedi temperory
                //vsolanki 2018-11-1---------------socket method   --------------------------
                //WalletMasterResponse walletMasterObj = new WalletMasterResponse();
                //walletMasterObj.AccWalletID = dWalletobj.AccWalletID;
                //walletMasterObj.Balance = dWalletobj.Balance;
                //walletMasterObj.WalletName = dWalletobj.Walletname;
                //walletMasterObj.PublicAddress = dWalletobj.PublicAddress;
                //walletMasterObj.AccWalletID = dWalletobj.AccWalletID;
                //walletMasterObj.CoinName = coinName;
                //_signalRService.OnWalletBalChange(walletMasterObj, coinName, dWalletobj.UserID.ToString(), 2);
                //var msg = EnResponseMessage.CreditWalletMsg;
                //msg = msg.Replace("#Coin#", coinName);
                //msg = msg.Replace("#TrnType#", routeTrnType.ToString());
                //msg = msg.Replace("#TrnNo#", TrnRefNo.ToString());
                //_signalRService.SendActivityNotification(msg, dWalletobj.UserID.ToString(), 2);
                // OnWalletBalChange(walletMasterObj, coinName,  Token);
                //-------------------------------


                //vsolanki 2018-11-1---------------socket method   --------------------------
                //WalletMasterResponse walletMasterObj1 = new WalletMasterResponse();
                //walletMasterObj.AccWalletID = cWalletObj.AccWalletID;
                //walletMasterObj.Balance = cWalletObj.Balance;
                //walletMasterObj.WalletName = cWalletObj.Walletname;
                //walletMasterObj.PublicAddress = cWalletObj.PublicAddress;
                //walletMasterObj.AccWalletID = cWalletObj.AccWalletID;
                //walletMasterObj.CoinName = coinName;
                //_signalRService.OnWalletBalChange(walletMasterObj1, coinName, Token);

                //OnWalletBalChange(walletMasterObj, coinName, Token);
                //-------------------------------



                //CreditWalletDrArryTrnID[] arryTrnID = new CreditWalletDrArryTrnID [1];
                //arryTrnID[0].Amount = amount;
                //arryTrnID[0].DrTrnRefNo = TrnRefNo;

                //resp = GetWalletCreditNew(coinName, timestamp, trnType, amount, cWalletObj.UserID, cWalletObj.AccWalletID,  arryTrnID, TrnRefNo,1,enWalletTranxOrderType.Credit,enServiceType.WalletService );
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = resp.TrnNo, Status = resp.Status, StatusMsg = resp.StatusMsg };

            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        protected WalletTransactionQueue InsertIntoWalletTransactionQueue(Guid Guid, enWalletTranxOrderType TrnType, decimal Amount, long TrnRefNo, DateTime TrnDate, DateTime? UpdatedDate,
            long WalletID, string WalletType, long MemberID, string TimeStamp, enTransactionStatus Status, string StatusMsg, enWalletTrnType enWalletTrnType)
        {
            WalletTransactionQueue walletTransactionQueue = new WalletTransactionQueue();
            // walletTransactionQueue.TrnNo = TrnNo;
            walletTransactionQueue.Guid = Guid;
            walletTransactionQueue.TrnType = TrnType;
            walletTransactionQueue.Amount = Amount;
            walletTransactionQueue.TrnRefNo = TrnRefNo;
            walletTransactionQueue.TrnDate = TrnDate;
            walletTransactionQueue.UpdatedDate = UpdatedDate;
            walletTransactionQueue.WalletID = WalletID;
            walletTransactionQueue.WalletType = WalletType;
            walletTransactionQueue.MemberID = MemberID;
            walletTransactionQueue.TimeStamp = TimeStamp;
            walletTransactionQueue.Status = Status;
            walletTransactionQueue.StatusMsg = StatusMsg;
            walletTransactionQueue.WalletTrnType = enWalletTrnType;
            return walletTransactionQueue;
        }

        protected int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType, enWalletTrnType wType)
        {
            var count = _walletRepository1.CheckTrnRefNo(TrnRefNo, TrnType, wType);
            return count;
        }

        public long GetWalletByAddress(string address)
        {
            try
            {
                var addressObj = _addressMstRepository.FindBy(e => e.Address == address).FirstOrDefault();
                if (addressObj == null)
                {
                    return 0;
                }
                else
                {
                    return addressObj.WalletId;
                }
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public WalletTransactionOrder InsertIntoWalletTransactionOrder(DateTime? UpdatedDate, DateTime TrnDate, long OWalletID, long DWalletID, decimal Amount, string WalletType, long OTrnNo, long DTrnNo, enTransactionStatus Status, string StatusMsg)
        {
            WalletTransactionOrder walletTransactionOrder = new WalletTransactionOrder();
            //walletTransactionOrder.OrderID = OrderID;
            walletTransactionOrder.UpdatedDate = UpdatedDate;
            walletTransactionOrder.TrnDate = TrnDate;
            walletTransactionOrder.OWalletID = OWalletID;
            walletTransactionOrder.DWalletID = DWalletID;
            walletTransactionOrder.Amount = Amount;
            walletTransactionOrder.WalletType = WalletType;
            walletTransactionOrder.OTrnNo = OTrnNo;
            walletTransactionOrder.DTrnNo = DTrnNo;
            walletTransactionOrder.Status = Status;
            walletTransactionOrder.StatusMsg = StatusMsg;
            return walletTransactionOrder;
        }

        public WalletLedger GetWalletLedgerObj(long WalletID, long toWalletID, decimal drAmount, decimal crAmount, enWalletTrnType trnType, enServiceType serviceType, long trnNo, string remarks, decimal currentBalance, byte status)
        {
            try
            {
                var walletLedger2 = new WalletLedger();
                walletLedger2.ServiceTypeID = serviceType;
                walletLedger2.TrnType = trnType;
                walletLedger2.CrAmt = crAmount;
                walletLedger2.CreatedBy = WalletID;
                walletLedger2.CreatedDate = UTC_To_IST();
                walletLedger2.DrAmt = drAmount;
                walletLedger2.TrnNo = trnNo;
                walletLedger2.Remarks = remarks;
                walletLedger2.Status = status;
                walletLedger2.TrnDate = UTC_To_IST();
                walletLedger2.UpdatedBy = WalletID;
                walletLedger2.WalletId = WalletID;
                walletLedger2.ToWalletId = toWalletID;
                if (drAmount > 0)
                {
                    walletLedger2.PreBal = currentBalance;
                    walletLedger2.PostBal = currentBalance - drAmount;
                }
                else
                {
                    walletLedger2.PreBal = currentBalance;
                    walletLedger2.PostBal = currentBalance + crAmount;
                }
                return walletLedger2;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public TransactionAccount GetTransactionAccount(long WalletID, short isSettled, long batchNo, decimal drAmount, decimal crAmount, long trnNo, string remarks, byte status)
        {
            try
            {
                var walletLedger2 = new TransactionAccount();
                walletLedger2.CreatedBy = WalletID;
                walletLedger2.CreatedDate = UTC_To_IST();
                walletLedger2.DrAmt = drAmount;
                walletLedger2.CrAmt = crAmount;
                walletLedger2.RefNo = trnNo;
                walletLedger2.Remarks = remarks;
                walletLedger2.Status = status;
                walletLedger2.TrnDate = UTC_To_IST();
                walletLedger2.UpdatedBy = WalletID;
                walletLedger2.WalletID = WalletID;
                walletLedger2.IsSettled = isSettled;
                walletLedger2.BatchNo = batchNo;
                return walletLedger2;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 2018-10-29
        public bool CheckUserBalance(long WalletId)
        {
            try
            {
                var wObjBal = GetUserBalance(WalletId);
                //var TA = _TransactionAccountsRepository.FindBy(item=>item.WalletID== WalletId);
                var crsum = _TransactionAccountsRepository.GetSum(e => e.WalletID == WalletId && e.IsSettled == 1, f => f.CrAmt);
                var drsum = _TransactionAccountsRepository.GetSum(e => e.WalletID == WalletId && e.IsSettled == 1, f => f.DrAmt);
                var total = crsum - drsum;
                if (total == wObjBal && total >= 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public decimal GetUserBalance(long walletId)
        {
            try
            {
                var obj = _commonRepository.GetById(walletId);
                return obj.Balance;
            }
            catch (Exception ex)
            {
                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //Uday 30-10-2018
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
