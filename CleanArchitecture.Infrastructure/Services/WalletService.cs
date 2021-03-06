﻿using System;
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
using System.Globalization;
using CleanArchitecture.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.Core.Entities.User;

namespace CleanArchitecture.Infrastructure.Services
{
    public class WalletService : BasePage, IWalletService
    {
        private readonly ILogger<WalletService> _log;
        private readonly ICommonRepository<WalletMaster> _commonRepository;
        private readonly ICommonRepository<WalletLimitConfiguration> _LimitcommonRepository;
        private readonly ICommonRepository<WalletLimitConfigurationMaster> _WalletLimitConfigurationMasterRepository;
        private readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyCommonRepository;
        private readonly ICommonRepository<WalletOrder> _walletOrderRepository;
        private readonly ICommonRepository<AddressMaster> _addressMstRepository;
        private readonly ICommonRepository<TrnAcBatch> _trnBatch;
        private readonly ICommonRepository<TradeBitGoDelayAddresses> _bitgoDelayRepository;
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepo;
        private readonly ICommonRepository<BeneficiaryMaster> _BeneficiarycommonRepository;
        private readonly ICommonRepository<UserPreferencesMaster> _UserPreferencescommonRepository;
        private readonly ICommonRepository<WalletLedger> _WalletLedgersRepo;
        private readonly ICommonRepository<MemberShadowBalance> _ShadowBalRepo;
        private readonly ICommonRepository<MemberShadowLimit> _ShadowLimitRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommonRepository<ConvertFundHistory> _ConvertFundHistory;

        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        private readonly IWalletRepository _walletRepository1;
        private readonly IWebApiRepository _webApiRepository;
        private readonly IWebApiSendRequest _webApiSendRequest;
        private readonly IGetWebRequest _getWebRequest;
        private readonly WebApiParseResponse _WebApiParseResponse;


        //vsolanki 8-10-2018 
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        private readonly ICommonRepository<WithdrawHistory> _WithdrawHistoryRepository;
        //readonly IBasePage _BaseObj;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        //vsolanki 10-10-2018 
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepository;
        private readonly ICommonRepository<TransactionAccount> _TransactionAccountsRepository;
        private readonly ICommonRepository<ChargeRuleMaster> _chargeRuleMaster;
        private readonly ICommonRepository<LimitRuleMaster> _limitRuleMaster;
        private readonly ISignalRService _signalRService;
        private readonly ICommonWalletFunction _commonWalletFunction;

        //private readonly IRepository<WalletTransactionOrder> _WalletAllowTrnRepository;
        //  private readonly ICommonRepository<WalletTransactionQueue> t;

        public WalletService(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository, WebApiParseResponse WebApiParseResponse,
            ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,
            IWebApiRepository webApiRepository, IWebApiSendRequest webApiSendRequest, ICommonRepository<ThirdPartyAPIConfiguration> thirdpartyCommonRepo,
            IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
            ILogger<BasePage> logger, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ICommonRepository<WalletAllowTrn> WalletAllowTrnRepository,
            ICommonRepository<WalletAllowTrn> WalletAllowTrnRepo, ICommonRepository<MemberShadowLimit> ShadowLimitRepo, ICommonRepository<MemberShadowBalance> ShadowBalRepo, ICommonRepository<WalletLimitConfigurationMaster> WalletConfigMasterRepo, ICommonRepository<BeneficiaryMaster> BeneficiaryMasterRepo, ICommonRepository<UserPreferencesMaster> UserPreferenceRepo, ICommonRepository<WalletLimitConfiguration> WalletLimitConfig,
            ICommonRepository<ChargeRuleMaster> chargeRuleMaster, ICommonRepository<LimitRuleMaster> limitRuleMaster, ICommonRepository<TransactionAccount> TransactionAccountsRepository, UserManager<ApplicationUser> userManager, ISignalRService signalRService, ICommonWalletFunction commonWalletFunction, ICommonRepository<ConvertFundHistory> ConvertFundHistory, ICommonRepository<WithdrawHistory> WithdrawHistoryRepository) : base(logger)
        {
            _log = log;
            _userManager = userManager;
            _commonRepository = commonRepository;
            _walletOrderRepository = walletOrderRepository;
            //_walletRepository = repository;
            _bitgoDelayRepository = bitgoDelayRepository;
            _trnBatch = BatchLogger;
            _walletRepository1 = walletRepository;
            _webApiRepository = webApiRepository;
            _webApiSendRequest = webApiSendRequest;
            _thirdPartyCommonRepository = thirdpartyCommonRepo;
            _getWebRequest = getWebRequest;
            _addressMstRepository = addressMaster;
            _WalletTypeMasterRepository = WalletTypeMasterRepository;
            _WalletAllowTrnRepository = WalletAllowTrnRepository;
            _WebApiParseResponse = WebApiParseResponse;
            //_walletLedgerRepository = walletledgerrepo;
            _WalletAllowTrnRepo = WalletAllowTrnRepo;
            _LimitcommonRepository = WalletLimitConfig;
            _BeneficiarycommonRepository = BeneficiaryMasterRepo;
            _UserPreferencescommonRepository = UserPreferenceRepo;
            _WalletLimitConfigurationMasterRepository = WalletConfigMasterRepo;
            _ShadowBalRepo = ShadowBalRepo;
            _ShadowLimitRepo = ShadowLimitRepo;
            _chargeRuleMaster = chargeRuleMaster;
            _limitRuleMaster = limitRuleMaster;
            _TransactionAccountsRepository = TransactionAccountsRepository;
            _signalRService = signalRService;
            _commonWalletFunction = commonWalletFunction;
            _ConvertFundHistory = ConvertFundHistory;
            _WithdrawHistoryRepository = WithdrawHistoryRepository;
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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                //_log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        //Rushabh 26-10-2018
        public long GetWalletID(string AccWalletID)
        {
            try
            {
                var obj = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID);
                if (obj != null)//Rita for object ref error
                    return obj.Id;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                throw ex;
            }
        }

        //Rushabh 27-10-2018
        public string GetAccWalletID(long WalletID)
        {
            try
            {
                var obj = _commonRepository.GetSingle(item => item.Id == WalletID);
                if (obj != null)//Rita for object ref error
                    return obj.AccWalletID;
                else
                    return "";

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                throw ex;
            }
        }

        //Rushabh 27-10-2018
        public enErrorCode CheckWithdrawalBene(long WalletID, string Name, string DestinationAddress, enWhiteListingBit bit)
        {
            try
            {
                var Walletobj = _commonRepository.GetSingle(item => item.Id == WalletID && item.Status == Convert.ToInt16(ServiceStatus.Active));
                if (Walletobj != null)
                {
                    var UserPrefobj = _UserPreferencescommonRepository.GetSingle(item => item.UserID == Walletobj.UserID);
                    if (UserPrefobj != null)
                    {
                        var Beneobj = _BeneficiarycommonRepository.GetSingle(item => item.WalletTypeID == Walletobj.WalletTypeID && item.Address == DestinationAddress && item.Status == Convert.ToInt16(ServiceStatus.Active));
                        if (UserPrefobj.IsWhitelisting == Convert.ToInt16(enWhiteListingBit.ON))
                        {
                            if (Beneobj != null)
                            {
                                if (Beneobj.Address == DestinationAddress && Beneobj.IsWhiteListed == Convert.ToInt16(enWhiteListingBit.ON))
                                {
                                    return enErrorCode.Success;
                                }
                                else
                                {
                                    return enErrorCode.AddressNotFoundOrWhitelistingBitIsOff;
                                }
                            }
                            return enErrorCode.BeneficiaryNotFound;
                        }
                        else
                        {
                            if (Beneobj != null)
                            {
                                if (Beneobj.Address == DestinationAddress)
                                {
                                    Beneobj.IsWhiteListed = Convert.ToInt16(bit);
                                    Beneobj.UpdatedBy = Walletobj.UserID;
                                    Beneobj.UpdatedDate = UTC_To_IST();
                                    _BeneficiarycommonRepository.Update(Beneobj);
                                    return enErrorCode.Success;
                                }
                                return enErrorCode.AddressNotMatch;
                            }
                            else
                            {
                                BeneficiaryMaster AddNew = new BeneficiaryMaster();
                                AddNew.IsWhiteListed = Convert.ToInt16(enWhiteListingBit.OFF);
                                AddNew.Status = Convert.ToInt16(ServiceStatus.Active);
                                AddNew.CreatedBy = Walletobj.UserID;
                                AddNew.CreatedDate = UTC_To_IST();
                                AddNew.UserID = Walletobj.UserID;
                                AddNew.Address = DestinationAddress;
                                AddNew.Name = Name;
                                AddNew.WalletTypeID = Walletobj.WalletTypeID;
                                AddNew = _BeneficiarycommonRepository.Add(AddNew);
                                return enErrorCode.Success;
                            }
                        }
                    }
                    //return enErrorCode.GlobalBitNotFound;
                    else
                    {
                        var Beneobj = _BeneficiarycommonRepository.GetSingle(item => item.WalletTypeID == Walletobj.WalletTypeID && item.Address == DestinationAddress && item.Status == Convert.ToInt16(ServiceStatus.Active));
                        if (Beneobj != null)
                        {
                            return enErrorCode.Success;
                        }
                        else
                        {
                            BeneficiaryMaster AddNew = new BeneficiaryMaster();
                            AddNew.IsWhiteListed = Convert.ToInt16(enWhiteListingBit.OFF);
                            AddNew.Status = Convert.ToInt16(ServiceStatus.Active);
                            AddNew.CreatedBy = Walletobj.UserID;
                            AddNew.CreatedDate = UTC_To_IST();
                            AddNew.UserID = Walletobj.UserID;
                            AddNew.Address = DestinationAddress;
                            AddNew.Name = Name;
                            AddNew.WalletTypeID = Walletobj.WalletTypeID;
                            AddNew = _BeneficiarycommonRepository.Add(AddNew);
                            return enErrorCode.Success;
                        }
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

        //public enErrorCode CheckShadowLimit(long WalletID, decimal Amount)
        //{
        //    try
        //    {
        //        var Walletobj = _commonRepository.GetSingle(item => item.Id == WalletID);
        //        if (Walletobj != null)
        //        {
        //            var Balobj = _ShadowBalRepo.GetSingle(item => item.WalletID == WalletID);
        //            if (Balobj != null)
        //            {
        //                if ((Balobj.ShadowAmount + Amount) <= Walletobj.Balance)
        //                {
        //                    return enErrorCode.Success;
        //                }
        //                return enErrorCode.InsufficientBalance;
        //            }
        //            else
        //            {
        //                var typeobj = _walletRepository1.GetTypeMappingObj(Walletobj.UserID);
        //                if (typeobj != 0)
        //                {
        //                    var Limitobj = _ShadowLimitRepo.GetSingle(item => item.MemberTypeId == typeobj);
        //                    if (Limitobj != null)
        //                    {
        //                        if ((Limitobj.ShadowLimitAmount + Amount) <= Walletobj.Balance)
        //                        {
        //                            return enErrorCode.Success;
        //                        }
        //                        return enErrorCode.InsufficientBalance;
        //                    }
        //                    return enErrorCode.NotFoundLimit;
        //                }
        //                return enErrorCode.MemberTypeNotFound;
        //            }
        //        }
        //        return enErrorCode.WalletNotFound;
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

        //        throw ex;
        //    }
        //}

        public enValidateWalletLimit ValidateWalletLimit(enTrnType TranType, decimal PerDayAmt, decimal PerHourAmt, decimal PerTranAmt, long WalletID)
        {
            try
            {
                var obj = _LimitcommonRepository.GetSingle(item => item.WalletId == WalletID && item.TrnType == Convert.ToInt16(TranType));
                if ((PerDayAmt <= obj.LimitPerDay) && (PerHourAmt <= obj.LimitPerHour) && (PerTranAmt <= obj.LimitPerTransaction))
                {
                    return enValidateWalletLimit.Success;
                }
                else
                {
                    return enValidateWalletLimit.Fail;
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                throw ex;
            }
        }

        public bool IsValidWallet(long walletId)
        {
            try
            {
                return _commonRepository.GetById(walletId).IsValid;

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);

                throw ex;
            }
        }

        public bool WalletBalanceCheck(decimal amount, string walletid)
        {
            try
            {
                var obj = _commonRepository.GetSingle(item => item.AccWalletID == walletid);
                if (obj != null)
                {
                    if (obj.Balance < amount)
                    {
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public CreateOrderResponse CreateOrder(CreateOrderRequest Order)
        {
            try
            {
                var response = new CreateOrderResponse();

                //WalletService walletServiceObj = WalletBalanceCheck(Order.OrderAmt, Order.OWalletMasterID);

                if (!IsValidWallet(Order.OWalletMasterID) == true)
                {
                    response.OrderID = 0;
                    response.ORemarks = "";
                    response.ReturnMsg = "Invalid Account";
                    response.ReturnCode = enResponseCodeService.Fail;
                    return response;
                }
                var orderItem = new WalletOrder()
                {
                    CreatedBy = 900, // temperory bind member not now
                    DeliveryAmt = Order.OrderAmt,
                    OrderDate = UTC_To_IST(),
                    OrderType = Order.OrderType,
                    OrderAmt = Order.OrderAmt,
                    DWalletMasterID = Order.DWalletMasterID,
                    OWalletMasterID = Order.OWalletMasterID,
                    Status = 0,
                    CreatedDate = UTC_To_IST(),
                    ORemarks = Order.ORemarks
                };
                _walletOrderRepository.Add(orderItem);
                response.OrderID = orderItem.Id;
                response.ORemarks = "Successfully Inserted";
                response.ErrorCode = enErrorCode.Success;
                response.ReturnMsg = "Successfully Inserted";
                response.ReturnCode = 0;
                return response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponse ProcessOrder(long RefNo, long DWalletID, long OWalletID, decimal amount, string remarks, enWalletTrnType enTrnType, enServiceType serviceType)
        {
            try
            {
                TransactionAccount tansAccObj = new TransactionAccount();
                TransactionAccount tansAccObj1 = new TransactionAccount();
                BizResponse bizResponse = new BizResponse();


                decimal balance;

                balance = GetUserBalance(DWalletID);
                if (amount < 0)
                {
                    //return false;
                    bizResponse.ErrorCode = enErrorCode.InvalidAmount;
                    bizResponse.ReturnMsg = "Invalid Amount";
                    bizResponse.ReturnCode = enResponseCodeService.Fail;
                    return bizResponse;
                }
                if (balance < amount)
                {
                    // return false;
                    bizResponse.ErrorCode = enErrorCode.InsufficientBalance;
                    bizResponse.ReturnMsg = "Insufficient Balance";
                    bizResponse.ReturnCode = enResponseCodeService.Fail;
                    return bizResponse;
                }
                var dWalletobj = _commonRepository.GetById(DWalletID);
                if (dWalletobj == null || dWalletobj.Status != 1)
                {
                    // return false;
                    bizResponse.ErrorCode = enErrorCode.InvalidWallet;
                    bizResponse.ReturnMsg = "Invalid Wallet";
                    bizResponse.ReturnCode = enResponseCodeService.Fail;
                    return bizResponse;
                }
                var oWalletobj = _commonRepository.GetById(OWalletID);
                if (oWalletobj == null || oWalletobj.Status != 1)
                {
                    bizResponse.ErrorCode = enErrorCode.InvalidWallet;
                    bizResponse.ReturnMsg = "Invalid Wallet";
                    bizResponse.ReturnCode = enResponseCodeService.Fail;

                    return bizResponse;
                }

                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                tansAccObj.BatchNo = batchObj.Id;
                tansAccObj.CrAmt = amount;
                tansAccObj.CreatedBy = DWalletID;
                tansAccObj.CreatedDate = UTC_To_IST();
                tansAccObj.DrAmt = 0;
                tansAccObj.IsSettled = 1;
                tansAccObj.RefNo = RefNo;
                tansAccObj.Remarks = remarks;
                tansAccObj.Status = 1;
                tansAccObj.TrnDate = UTC_To_IST();
                tansAccObj.UpdatedBy = DWalletID;
                tansAccObj.WalletID = OWalletID;
                //tansAccObj = _trnxAccount.Add(tansAccObj);

                tansAccObj1 = new TransactionAccount();
                tansAccObj1.BatchNo = batchObj.Id;
                tansAccObj1.CrAmt = 0;
                tansAccObj1.CreatedBy = DWalletID;
                tansAccObj1.CreatedDate = UTC_To_IST();
                tansAccObj1.DrAmt = amount;
                tansAccObj1.IsSettled = 1;
                tansAccObj1.RefNo = RefNo;
                tansAccObj1.Remarks = remarks;
                tansAccObj1.Status = 1;
                tansAccObj1.TrnDate = UTC_To_IST();
                tansAccObj1.UpdatedBy = DWalletID;
                tansAccObj1.WalletID = DWalletID;
                //tansAccObj = _trnxAccount.Add(tansAccObj);

                dWalletobj.DebitBalance(amount);
                oWalletobj.CreditBalance(amount);

                //_walletRepository.Update(dWalletobj);
                //_walletRepository.Update(oWalletobj);

                var walletLedger = new WalletLedger();
                walletLedger.ServiceTypeID = serviceType;
                walletLedger.TrnType = enTrnType;
                walletLedger.CrAmt = 0;
                walletLedger.CreatedBy = DWalletID;
                walletLedger.CreatedDate = UTC_To_IST();
                walletLedger.DrAmt = amount;
                walletLedger.TrnNo = RefNo;
                walletLedger.Remarks = remarks;
                walletLedger.Status = 1;
                walletLedger.TrnDate = UTC_To_IST();
                walletLedger.UpdatedBy = DWalletID;
                walletLedger.WalletId = DWalletID;
                walletLedger.ToWalletId = OWalletID;
                walletLedger.PreBal = dWalletobj.Balance;
                walletLedger.PostBal = dWalletobj.Balance - amount;
                //walletLedger = _walletLedgerRepository.Add(walletLedger);

                var walletLedger2 = new WalletLedger();
                walletLedger2.ServiceTypeID = serviceType;
                walletLedger2.TrnType = enTrnType;
                walletLedger2.CrAmt = amount;
                walletLedger2.CreatedBy = DWalletID;
                walletLedger2.CreatedDate = UTC_To_IST();
                walletLedger2.DrAmt = 0;
                walletLedger2.TrnNo = RefNo;
                walletLedger2.Remarks = remarks;
                walletLedger2.Status = 1;
                walletLedger2.TrnDate = UTC_To_IST();
                walletLedger2.UpdatedBy = DWalletID;
                walletLedger2.WalletId = OWalletID;
                walletLedger2.ToWalletId = DWalletID;
                walletLedger2.PreBal = oWalletobj.Balance;
                walletLedger2.PostBal = oWalletobj.Balance - amount;

                _walletRepository1.WalletOperation(walletLedger, walletLedger2, tansAccObj, tansAccObj1, dWalletobj, oWalletobj);
                bizResponse.ErrorCode = enErrorCode.Success;
                bizResponse.ReturnMsg = "Success";
                bizResponse.ReturnCode = enResponseCodeService.Success;
                return bizResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public CreateWalletAddressRes GenerateAddress(string walletID, string coin, string Token, int GenaratePendingbit = 0)
        {
            try
            {
                ThirdPartyAPIRequest thirdPartyAPIRequest;
                //WebApiParseResponse _WebApiParseResponse;
                TradeBitGoDelayAddresses delayAddressesObj, delayGeneratedAddressesObj;
                List<TransactionProviderResponse> transactionProviderResponses;
                //WalletMaster walletMaster = _commonRepository.GetById(walletID);
                WalletMaster walletMaster = _commonRepository.GetSingle(item => item.AccWalletID == walletID && item.Status != Convert.ToInt16(ServiceStatus.Disable));
                AddressMaster addressMaster;
                string address = "";
                string CoinSpecific = null;
                string TrnID = null;
                string Respaddress = null;

                var wallettype = _WalletTypeMasterRepository.GetSingle(t => t.WalletTypeName == coin);
                //var user = _userManager.GetUserAsync(HttpContext.User);
                //if (user == null)
                //{
                //    Response.ReturnCode = enResponseCode.Fail;
                //    Response.ReturnMsg = EnResponseMessage.StandardLoginfailed;
                //    Response.ErrorCode = enErrorCode.StandardLoginfailed;
                //}

                if (wallettype.Id != walletMaster.WalletTypeID)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }
                if (walletMaster == null)
                {
                    //return false enResponseCodeService.Fail
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }
                else if (walletMaster.Status != Convert.ToInt16(ServiceStatus.Active))
                {
                    //return false
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }

                transactionProviderResponses = _webApiRepository.GetProviderDataList(new TransactionApiConfigurationRequest { SMSCode = coin.ToLower(), amount = 0, APIType = enWebAPIRouteType.TransactionAPI, trnType = Convert.ToInt32(enTrnType.Generate_Address) });
                if (transactionProviderResponses == null || transactionProviderResponses.Count == 0)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress, ReturnCode = enResponseCode.Fail, ReturnMsg = "Please try after sometime." };
                }
                if (transactionProviderResponses[0].ThirPartyAPIID == 0)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ItemOrThirdprtyNotFound };
                }

                ThirdPartyAPIConfiguration thirdPartyAPIConfiguration = _thirdPartyCommonRepository.GetById(transactionProviderResponses[0].ThirPartyAPIID);
                if (thirdPartyAPIConfiguration == null || transactionProviderResponses.Count == 0)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ItemOrThirdprtyNotFound };
                }
                thirdPartyAPIRequest = _getWebRequest.MakeWebRequest(transactionProviderResponses[0].RouteID, transactionProviderResponses[0].ThirPartyAPIID, transactionProviderResponses[0].SerProDetailID);
                string apiResponse = _webApiSendRequest.SendAPIRequestAsync(thirdPartyAPIRequest.RequestURL, thirdPartyAPIRequest.RequestBody, thirdPartyAPIConfiguration.ContentType, 180000, thirdPartyAPIRequest.keyValuePairsHeader, thirdPartyAPIConfiguration.MethodType);
                // parse response logic 


                WebAPIParseResponseCls ParsedResponse = _WebApiParseResponse.TransactionParseResponse(apiResponse, transactionProviderResponses[0].ThirPartyAPIID);
                Respaddress = ParsedResponse.TrnRefNo;
                if (!string.IsNullOrEmpty(apiResponse) && thirdPartyAPIRequest.DelayAddress == 1)
                {

                    //WebAPIParseResponseCls ParsedResponse = _WebApiParseResponse.TransactionParseResponse(apiResponse, transactionProviderResponses[0].ThirPartyAPIID);
                    //Respaddress = ParsedResponse.StatusMsg;
                    delayAddressesObj = GetTradeBitGoDelayAddresses(0, walletMaster.WalletTypeID, ParsedResponse.StatusMsg, "", thirdPartyAPIRequest.walletID, walletMaster.CreatedBy, ParsedResponse.Param1, 0, 0, coin);
                    delayAddressesObj = _bitgoDelayRepository.Add(delayAddressesObj);


                    if (GenaratePendingbit == 0)
                    {
                        delayGeneratedAddressesObj = _walletRepository1.GetUnassignedETH();

                        if (delayGeneratedAddressesObj == null)
                        {
                            return new CreateWalletAddressRes { address = Respaddress, ErrorCode = enErrorCode.UnAssignedAddressFetchFail, ReturnCode = enResponseCode.Fail, ReturnMsg = "please try after some time" };
                        }
                        address = delayGeneratedAddressesObj.Address;
                        Respaddress = delayGeneratedAddressesObj.Address;

                        delayGeneratedAddressesObj.WalletId = walletMaster.Id;
                        delayGeneratedAddressesObj.UpdatedBy = walletMaster.UserID;
                        delayGeneratedAddressesObj.UpdatedDate = UTC_To_IST();
                        _bitgoDelayRepository.Update(delayGeneratedAddressesObj);

                    }
                    else
                    {

                        return new CreateWalletAddressRes { ErrorCode = enErrorCode.Success, ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CreateAddressSuccessMsg };
                    }
                }
                // if (!string.IsNullOrEmpty(apiResponse))
                // //{
                // //    WebAPIParseResponseCls ParsedResponse = _WebApiParseResponse.TransactionParseResponse(apiResponse, transactionProviderResponses[0].ThirPartyAPIID);
                // //    Respaddress = ParsedResponse.TrnRefNo;
                //// }

                if (!string.IsNullOrEmpty(Respaddress))
                {
                    addressMaster = GetAddressObj(walletMaster.Id, transactionProviderResponses[0].ServiceProID, Respaddress, "Self Address", walletMaster.UserID, 0, 1);
                    addressMaster = _addressMstRepository.Add(addressMaster);
                    var msg = EnResponseMessage.GenerateAddressNotification;
                    msg = msg.Replace("#WalletName#", walletMaster.Walletname);
                    _signalRService.SendActivityNotification(msg, Token);
                    string responseString = Respaddress;
                    return new CreateWalletAddressRes { address = Respaddress, ErrorCode = enErrorCode.Success, ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CreateAddressSuccessMsg };
                    //return respObj;
                }
                else
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.AddressGenerationFailed, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.CreateWalletFailMsg };
                }

                // code need to be added
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog("GenerateAddress ", "WalletService", ex);
                //HelperForLog.WriteLogIntoFile("CombineAllInitTransactionAsync", ControllerName, "Transaction/Withdraw Process Start" + "##TrnNo:" + Req.TrnNo);
                throw ex;
            }
        }

        public AddressMaster GetAddressObj(long walletID, long serproID, string address, string addressName, long createdBy, byte isDefaultAdd, short status)
        {
            try
            {
                AddressMaster addressMaster = new AddressMaster();
                addressMaster.Address = address;
                // addressMaster.CoinName = coinName;
                addressMaster.AddressLable = addressName;
                addressMaster.CreatedBy = createdBy;
                addressMaster.CreatedDate = UTC_To_IST();
                addressMaster.IsDefaultAddress = isDefaultAdd;
                addressMaster.SerProID = serproID;
                addressMaster.Status = status;
                addressMaster.WalletId = walletID;
                return addressMaster;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public TradeBitGoDelayAddresses GetTradeBitGoDelayAddresses(long walletID, long WalletTypeId, string TrnID, string address, string BitgoWalletId, long createdBy, string CoinSpecific, short status, byte generatebit, string coin)
        {
            try
            {
                TradeBitGoDelayAddresses addressMaster = new TradeBitGoDelayAddresses
                {
                    CoinSpecific = CoinSpecific,
                    Address = address,
                    BitgoWalletId = BitgoWalletId,
                    CoinName = coin,
                    CreatedBy = createdBy,
                    CreatedDate = UTC_To_IST(),
                    GenerateBit = generatebit,
                    Status = status,
                    TrnID = TrnID,
                    WalletId = walletID,
                    WalletTypeId = WalletTypeId
                };

                return addressMaster;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public WalletMaster GetWalletMaster(long WalletTypeId, string walletName, bool isValid, short status, long createdBy, string coinname)
        {
            try
            {
                WalletMaster addressMaster = new WalletMaster
                {
                    //CoinName = coinname,
                    CreatedBy = createdBy,
                    CreatedDate = UTC_To_IST(),
                    Status = status,
                    Balance = 0,
                    IsValid = isValid,
                    Walletname = walletName,
                    WalletTypeID = WalletTypeId
                };

                return addressMaster;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public DepositHistory GetDepositHistory(string fromAddress, string toAddress, string coinName, string trnID, long confirmations, decimal amount,
        short status, string statusMsg, string confirmedTime, string UnconfirmedTime, string epochtimePure, byte isProcessing, long serproid, long createdby)
        {
            try
            {
                DepositHistory dh = new DepositHistory
                {
                    Address = toAddress,
                    Confirmations = confirmations,
                    IsProcessing = isProcessing,
                    SerProID = serproid,
                    SMSCode = coinName,
                    TrnID = trnID,
                    CreatedBy = createdby,
                    CreatedDate = UTC_To_IST(),
                    TimeEpoch = confirmedTime,
                    EpochTimePure = epochtimePure,
                    Status = status,
                    StatusMsg = statusMsg

                };
                return dh;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 8-10-2018 get coin list from WalletTypeMaster table
        public IEnumerable<WalletTypeMaster> GetWalletTypeMaster()
        {
            try
            {
                //  IEnumerable<WalletTypeMasterRes> coinlist = new List<WalletTypeMasterRes>();
                IEnumerable<WalletTypeMaster> coin = new List<WalletTypeMaster>();

                coin = _WalletTypeMasterRepository.FindBy(item => item.Status == Convert.ToInt16(ServiceStatus.Active));

                return coin;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public string RandomGenerateAccWalletId(long userID, byte isDefaultWallet)
        {
            try
            {
                long maxValue = 9999999999;
                long minValue = 1000000000;
                long x = (long)Math.Round(random.NextDouble() * (maxValue - minValue - 1)) + minValue;
                string userIDStr = x.ToString() + userID.ToString().PadLeft(5, '0') + isDefaultWallet.ToString();
                return userIDStr;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 10-10-2018 Insert into WalletMaster table
        public CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId, string accessToken = null, int isBaseService = 0)
        {
            bool IsValid = true;
            decimal Balance = 0;
            string PublicAddress = "";
            WalletMaster walletMaster = new WalletMaster();
            CreateWalletResponse createWalletResponse = new CreateWalletResponse();
            try
            {

                var walletMasters = _WalletTypeMasterRepository.GetSingle(item => item.WalletTypeName == CoinName);
                if (walletMasters == null)
                {
                    createWalletResponse.ReturnCode = enResponseCode.Fail;
                    createWalletResponse.ReturnMsg = EnResponseMessage.InvalidCoin;
                    createWalletResponse.ErrorCode = enErrorCode.InvalidCoinName;
                    return createWalletResponse;
                }


                //add data in walletmaster tbl
                walletMaster.Walletname = Walletname;
                walletMaster.IsValid = IsValid;
                walletMaster.UserID = userId;
                walletMaster.WalletTypeID = walletMasters.Id;
                walletMaster.Balance = Balance;
                walletMaster.PublicAddress = PublicAddress;
                walletMaster.IsDefaultWallet = IsDefaultWallet;
                walletMaster.CreatedBy = userId;
                walletMaster.CreatedDate = UTC_To_IST();
                walletMaster.Status = Convert.ToInt16(ServiceStatus.Active);
                walletMaster.AccWalletID = RandomGenerateAccWalletId(userId, IsDefaultWallet);
                walletMaster = _commonRepository.Add(walletMaster);

                //add data in WalletAllowTrn tbl
                for (int i = 0; i < AllowTrnType.Length; i++)
                {
                    WalletAllowTrn w = new WalletAllowTrn();
                    w.CreatedDate = UTC_To_IST();
                    w.CreatedBy = userId;
                    w.Status = Convert.ToInt16(ServiceStatus.Active);
                    w.WalletId = walletMaster.Id;
                    w.TrnType = Convert.ToByte(AllowTrnType[i]);
                    _WalletAllowTrnRepository.Add(w);
                }
                //genrate address and update in walletmaster
                if (isBaseService == 0)    //uday 25-10-2018 When Service is add create default wallet of org not generate the address
                {
                    var addressClass = GenerateAddress(walletMaster.AccWalletID, CoinName, accessToken);
                    if (addressClass.address != null)
                    {
                        walletMaster.WalletPublicAddress(addressClass.address);
                    }
                    //walletMaster.PublicAddress = addressClass.address;
                    walletMaster.WalletPublicAddress("NotGenerate");
                    _commonRepository.Update(walletMaster);
                }
                //vsolanki 26-10-2018 insert and limitConfigration
                _walletRepository1.GetSetLimitConfigurationMaster(AllowTrnType, userId, walletMaster.Id);

                //vsolanki 25-10-2018 add Limit for Wallet
                //var OrgWallet = _commonRepository.GetSingle(item => item.UserID == userId && item.WalletTypeID == walletMasters.Id);
                //WalletLimitConfigurationReq request = new WalletLimitConfigurationReq();
                //var limitConfig = _LimitcommonRepository.GetSingle(item => item.WalletId == 1);
                //get record from limitmaster 
                //var limitConfig= _WalletLimitConfigurationMasterRepository.GetSingle(item=>item.TrnType== AllowTrnType[0]);

                //request.EndTime = limitConfig.EndTime;
                //request.StartTime = limitConfig.StartTime;
                //request.LimitPerDay = limitConfig.LimitPerDay;
                //request.LimitPerHour = limitConfig.LimitPerHour;
                //request.LimitPerTransaction = limitConfig.LimitPerTransaction;
                //enWalletLimitType trntype = (enWalletLimitType)Enum.ToObject(typeof(enWalletLimitType), limitConfig.TrnType);
                //request.TrnType = trntype;
                //request.EndTime = limitConfig.EndTime;
                ////insert into limit tbl
                //CleanArchitecture.Core.ViewModels.WalletOperations.LimitResponse limits = SetWalletLimitConfig(walletMaster.AccWalletID, request,userId);
                //set the response object value

                createWalletResponse.AccWalletID = walletMaster.AccWalletID;
                createWalletResponse.PublicAddress = walletMaster.PublicAddress;
                createWalletResponse.Limits = _walletRepository1.GetWalletLimitResponse(walletMaster.AccWalletID);
                createWalletResponse.ReturnCode = enResponseCode.Success;
                createWalletResponse.ReturnMsg = EnResponseMessage.CreateWalletSuccessMsg;
                createWalletResponse.ErrorCode = enErrorCode.Success;

                if (accessToken != null)
                {
                    try
                    {
                        var msg = EnResponseMessage.NewCreateWalletSuccessMsg;
                        msg = msg.Replace("#WalletName#", walletMaster.Walletname);
                        _signalRService.SendActivityNotification(msg, accessToken);
                    }
                    catch (Exception ex)
                    {
                        HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                    }
                }
                return createWalletResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponseClass DebitBalance(long userID, long WalletID, decimal amount, int walletTypeID, enWalletTrnType wtrnType, enTrnType trnType, enServiceType serviceType, long trnNo, string smsCode)
        {
            WalletMaster dWalletobj;
            string remarks = "";
            try
            {
                if (WalletID == 0 && (walletTypeID == 0 || userID == 0))
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidAmount };
                }
                if (WalletID == 0)
                {
                    IEnumerable<WalletMaster> walletMasters = _commonRepository.FindBy(e => e.IsValid == true && e.UserID == userID && e.WalletTypeID == walletTypeID && e.IsDefaultWallet == 1);
                    if (walletMasters == null)
                    {
                        return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.DefaultWallet404, ErrorCode = enErrorCode.DefaultWalletNotFound };
                    }
                    List<WalletMaster> list = walletMasters.ToList();
                    dWalletobj = list[0];
                }
                else
                {
                    dWalletobj = _commonRepository.GetById(WalletID);
                    if (dWalletobj == null)
                    {
                        return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWalletId };
                    }
                    if (dWalletobj.Status != 1 || dWalletobj.IsValid == false)
                    {
                        return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                    }
                }
                if (wtrnType != enWalletTrnType.Dr_Sell_Trade) // currently added code for only sell trade 
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTrnType, ErrorCode = enErrorCode.InvalidTrnType };
                }
                if (amount <= 0)
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount };
                }
                if (dWalletobj.Balance < amount)
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficientBal, ErrorCode = enErrorCode.InsufficientBalance };
                }
                WalletAllowTrn walletAllowTrn = _WalletAllowTrnRepo.GetSingle(e => e.TrnType == (byte)trnType && e.WalletId == dWalletobj.Id && e.Status == 1);
                if (walletAllowTrn == null)
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.NotAllowedTrnType, ErrorCode = enErrorCode.TrnTypeNotAllowed };
                }
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                if (batchObj == null)
                {
                    return new BizResponseClass { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.BatchNoFailed, ErrorCode = enErrorCode.BatchNoGenerationFailed };
                }
                if (wtrnType == enWalletTrnType.Dr_Sell_Trade)
                {
                    remarks = "Sell Trade [" + smsCode + "] TrnNo:" + trnNo;
                }
                else
                {
                    remarks = "Debit of TrnNo:" + trnNo;
                }

                WalletLedger walletLedger = GetWalletLedger(WalletID, 0, amount, 0, wtrnType, serviceType, trnNo, remarks, dWalletobj.Balance, 1);
                TransactionAccount tranxAccount = GetTransactionAccount(WalletID, 1, batchObj.Id, amount, 0, trnNo, remarks, 1);
                dWalletobj.DebitBalance(amount);
                _walletRepository1.WalletDeduction(walletLedger, tranxAccount, dWalletobj);
                return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CommSuccessMsgInternal, ErrorCode = enErrorCode.Success };

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public WalletLedger GetWalletLedger(long WalletID, long toWalletID, decimal drAmount, decimal crAmount, enWalletTrnType trnType, enServiceType serviceType, long trnNo, string remarks, decimal currentBalance, byte status)
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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 12-10-2018 Select WalletMaster table 
        public ListWalletResponse ListWallet(long userid)
        {
            ListWalletResponse listWalletResponse = new ListWalletResponse();
            try
            {
                var walletResponse = _walletRepository1.ListWalletMasterResponse(userid);
                if (walletResponse.Count == 0)
                {
                    listWalletResponse.ReturnCode = enResponseCode.Fail;
                    listWalletResponse.ReturnMsg = EnResponseMessage.NotFound;
                    listWalletResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    listWalletResponse.Wallets = walletResponse;
                    listWalletResponse.ReturnCode = enResponseCode.Success;
                    listWalletResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    listWalletResponse.ErrorCode = enErrorCode.Success;

                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                listWalletResponse.ReturnCode = enResponseCode.InternalError;
                return listWalletResponse;
            }
        }

        //vsolanki 12-10-2018 Select WalletMaster table ByCoin
        public ListWalletResponse GetWalletByCoin(long userid, string coin)
        {
            ListWalletResponse listWalletResponse = new ListWalletResponse();
            try
            {
                var walletResponse = _walletRepository1.GetWalletMasterResponseByCoin(userid, coin);
                var UserPrefobj = _UserPreferencescommonRepository.FindBy(item => item.UserID == userid && item.Status == Convert.ToInt16(ServiceStatus.Active)).FirstOrDefault();
                if (walletResponse.Count == 0)
                {
                    listWalletResponse.ReturnCode = enResponseCode.Fail;
                    listWalletResponse.ReturnMsg = EnResponseMessage.NotFound;
                    listWalletResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    if (UserPrefobj != null)
                    {
                        listWalletResponse.IsWhitelisting = UserPrefobj.IsWhitelisting;
                    }
                    else
                    {
                        listWalletResponse.IsWhitelisting = 0;
                    }
                    listWalletResponse.Wallets = walletResponse;
                    listWalletResponse.ReturnCode = enResponseCode.Success;
                    listWalletResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    listWalletResponse.ErrorCode = enErrorCode.Success;

                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                listWalletResponse.ReturnCode = enResponseCode.InternalError;
                return listWalletResponse;
            }
        }

        //vsolanki 12-10-2018 Select WalletMaster table ByCoin
        public ListWalletResponse GetWalletById(long userid, string coin, string walletId)
        {
            ListWalletResponse listWalletResponse = new ListWalletResponse();
            try
            {
                var walletResponse = _walletRepository1.GetWalletMasterResponseById(userid, coin, walletId);
                if (walletResponse.Count == 0)
                {
                    listWalletResponse.ReturnCode = enResponseCode.Fail;
                    listWalletResponse.ReturnMsg = EnResponseMessage.NotFound;
                    listWalletResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    listWalletResponse.Wallets = walletResponse;
                    listWalletResponse.ReturnCode = enResponseCode.Success;
                    listWalletResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    listWalletResponse.ErrorCode = enErrorCode.Success;
                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                listWalletResponse.ReturnCode = enResponseCode.InternalError;
                return listWalletResponse;
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

        public WalletTransactionQueue InsertIntoWalletTransactionQueue(Guid Guid, enWalletTranxOrderType TrnType, decimal Amount, long TrnRefNo, DateTime TrnDate, DateTime? UpdatedDate,
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

        public WalletDrCrResponse GetWalletDeductionNew(string coinName, string timestamp, enWalletTranxOrderType orderType, decimal amount, long userID, string accWalletID, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType, enTrnType routeTrnType, string Token = "")
        {
            try
            {
                WalletMaster dWalletobj;
                string remarks = "";
                WalletTypeMaster walletTypeMaster;
                WalletTransactionQueue objTQ;
                //long walletTypeID;
                WalletDrCrResponse resp = new WalletDrCrResponse();
                HelperForLog.WriteLogIntoFile("GetWalletDeductionNew", "WalletService", "timestamp:" + timestamp + "," + "coinName:" + coinName + ",accWalletID=" + accWalletID + ",TrnRefNo=" + TrnRefNo.ToString() + ",userID=" + userID + ",amount=" + amount.ToString());
                if (string.IsNullOrEmpty(accWalletID) || coinName == string.Empty || userID == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidWalletOrUserIDorCoinName };
                }
                walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                if (walletTypeMaster == null)
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName },"Debit");
                }
                dWalletobj = _commonRepository.GetSingle(e => e.UserID == userID && e.WalletTypeID == walletTypeMaster.Id && e.AccWalletID == accWalletID);
                if (dWalletobj == null)
                {
                    //tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid().ToString(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 2, EnResponseMessage.InvalidWallet);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet }, "Debit");
                }
                if (dWalletobj.Status != 1 || dWalletobj.IsValid == false)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }

                if (!CheckUserBalance(dWalletobj.Id))
                {
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.BalMismatch, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.SettedBalanceMismatch, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                if (orderType != enWalletTranxOrderType.Debit) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTrnType, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTrnType, ErrorCode = enErrorCode.InvalidTrnType, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                if (TrnRefNo == 0) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                if (amount <= 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                if (dWalletobj.Balance < amount)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InsufficantBal, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficantBal, ErrorCode = enErrorCode.InsufficantBal, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                if (_commonWalletFunction.CheckShadowLimit(dWalletobj.Id, amount) != enErrorCode.Success)
                {
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InsufficantBal, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ShadowLimitExceed, ErrorCode = enErrorCode.ShadowBalanceExceed, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }
                //vsolanki 208-11-1
                var charge = GetServiceLimitChargeValue(routeTrnType, coinName);
                if (charge.MaxAmount < amount && charge.MinAmount > amount && charge.MaxAmount != 0 && charge.MinAmount != 0)
                {
                    var msg1 = EnResponseMessage.ProcessTrn_AmountBetweenMinMaxMsg;
                    msg1 = msg1.Replace("@MIN", charge.MinAmount.ToString());
                    msg1 = msg1.Replace("@MAX", charge.MaxAmount.ToString());
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, msg1, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = msg1, ErrorCode = enErrorCode.ProcessTrn_AmountBetweenMinMax }, "Debit");
                }

                int count = CheckTrnRefNo(TrnRefNo, orderType, trnType);
                if (count != 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.AlredyExist, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");
                }

                objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 0, "Inserted", trnType);
                objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                remarks = "Debit for TrnNo:" + objTQ.TrnNo;
                dWalletobj = _commonRepository.GetById(dWalletobj.Id);
                WalletLedger walletLedger = GetWalletLedger(dWalletobj.Id, 0, amount, 0, trnType, serviceType, objTQ.TrnNo, remarks, dWalletobj.Balance, 1);
                TransactionAccount tranxAccount = GetTransactionAccount(dWalletobj.Id, 1, batchObj.Id, amount, 0, objTQ.TrnNo, remarks, 1);
                dWalletobj.DebitBalance(amount);
                objTQ.Status = enTransactionStatus.Hold;
                objTQ.StatusMsg = "Hold";
                _walletRepository1.WalletDeductionwithTQ(walletLedger, tranxAccount, dWalletobj, objTQ);
                //vsolanki 2018-11-1---------------socket method   --------------------------
                WalletMasterResponse walletMasterObj = new WalletMasterResponse();
                walletMasterObj.AccWalletID = dWalletobj.AccWalletID;
                walletMasterObj.Balance = dWalletobj.Balance;
                walletMasterObj.WalletName = dWalletobj.Walletname;
                walletMasterObj.PublicAddress = dWalletobj.PublicAddress;
                walletMasterObj.IsDefaultWallet = dWalletobj.IsDefaultWallet;
                walletMasterObj.CoinName = coinName;

                _signalRService.OnWalletBalChange(walletMasterObj, coinName, Token);
                var msg = EnResponseMessage.DebitWalletMsg;
                msg = msg.Replace("#Coin#", coinName);
                msg = msg.Replace("#TrnType#", routeTrnType.ToString());
                msg = msg.Replace("#TrnNo#", TrnRefNo.ToString());
                _signalRService.SendActivityNotification(msg, Token);
                //-------------------------------
                return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg }, "Debit");

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }


        public int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType, enWalletTrnType wType)
        {
            var count = _walletRepository1.CheckTrnRefNo(TrnRefNo, TrnType, wType);
            return count;
        }

        public WalletDrCrResponse GetWalletCreditNew(string coinName, string timestamp, enWalletTrnType trnType, decimal TotalAmount, long userID, string crAccWalletID, CreditWalletDrArryTrnID[] arryTrnID, long TrnRefNo, short isFullSettled, enWalletTranxOrderType orderType, enServiceType serviceType, enTrnType routeTrnType, string Token = "")
        {
            WalletTransactionQueue tqObj = new WalletTransactionQueue();
            WalletTransactionOrder woObj = new WalletTransactionOrder();
            try
            {
                WalletMaster cWalletobj;
                string remarks = "";
                WalletTypeMaster walletTypeMaster;
                //long walletTypeID;
                WalletDrCrResponse resp = new WalletDrCrResponse();
                HelperForLog.WriteLogIntoFile("GetWalletCreditNew", "WalletService", "timestamp:" + timestamp + "," + "coinName:" + coinName + ",TrnRefNo=" + TrnRefNo.ToString() + ",userID=" + userID + ",amount=" + TotalAmount.ToString());

                if (string.IsNullOrEmpty(crAccWalletID) || coinName == string.Empty || userID == 0)
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidWalletOrUserIDorCoinName },"Credit");
                }
                walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                if (walletTypeMaster == null)
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName }, "Credit");
                }
                cWalletobj = _commonRepository.GetSingle(e => e.UserID == userID && e.WalletTypeID == walletTypeMaster.Id && e.AccWalletID == crAccWalletID);
                if (cWalletobj == null)
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.UserIDWalletIDDidNotMatch }, "Credit");
                }
                if (cWalletobj.Status != 1 || cWalletobj.IsValid == false)
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet }, "Credit");
                }
                if (orderType != enWalletTranxOrderType.Credit) // buy 
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTrnType, ErrorCode = enErrorCode.InvalidTrnType }, "Credit");
                }

                //WalletTransactionQueue
                if (TrnRefNo == 0) // buy
                {
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo }, "Credit");
                }
                if (!CheckUserBalance(cWalletobj.Id))
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.BalMismatch, trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.SettedBalanceMismatch, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg }, "Credit");
                }
                if (TotalAmount <= 0)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg }, "Credit");
                }
                int count = CheckTrnRefNoForCredit(TrnRefNo, enWalletTranxOrderType.Debit); // check whether for this refno wallet is pre decuted or not
                if (count == 0)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo, trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist }, "Credit");
                }
                bool checkArray = CheckarryTrnID(arryTrnID, coinName);// check whether all array dtrnrefno of same wallet and they are hold (not assigned to other order)
                if (checkArray == true)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, 0, "Inserted", trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    for (int w = 0; w <= arryTrnID.Length - 1; w++)
                    {
                        woObj = InsertIntoWalletTransactionOrder(null, UTC_To_IST(), cWalletobj.Id, arryTrnID[w].dWalletId, arryTrnID[w].Amount, coinName, tqObj.TrnNo, arryTrnID[w].DrTQTrnNo, 0, "Inserted");
                        woObj = _walletRepository1.AddIntoWalletTransactionOrder(woObj, 1);
                        arryTrnID[w].OrderID = woObj.OrderID;
                    }
                }
                else if (checkArray == false)//fail
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, "Amount and DebitRefNo matching failure", trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);

                    return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg }, "Credit");
                }
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                remarks = "Credit for TrnNo:" + tqObj.TrnNo;

                WalletLedger walletLedger = GetWalletLedger(cWalletobj.Id, 0, 0, TotalAmount, trnType, serviceType, tqObj.TrnNo, remarks, cWalletobj.Balance, 1);
                TransactionAccount tranxAccount = GetTransactionAccount(cWalletobj.Id, 1, batchObj.Id, 0, TotalAmount, tqObj.TrnNo, remarks, 1);
                cWalletobj.CreditBalance(TotalAmount);
                //var objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, 1, "Updated");
                tqObj.Status = enTransactionStatus.Success;
                tqObj.StatusMsg = "Success.";
                tqObj.UpdatedDate = UTC_To_IST();
                _walletRepository1.WalletCreditwithTQ(walletLedger, tranxAccount, cWalletobj, tqObj, arryTrnID);

                //vsolanki 2018-11-1---------------socket method   --------------------------
                WalletMasterResponse walletMasterObj = new WalletMasterResponse();
                walletMasterObj.AccWalletID = cWalletobj.AccWalletID;
                walletMasterObj.Balance = cWalletobj.Balance;
                walletMasterObj.WalletName = cWalletobj.Walletname;
                walletMasterObj.PublicAddress = cWalletobj.PublicAddress;
                walletMasterObj.IsDefaultWallet = cWalletobj.IsDefaultWallet;
                walletMasterObj.CoinName = coinName;

                //_signalRService.OnWalletBalChange(walletMasterObj, coinName, Token);
                // ntrivedi 03-11-2018
                var msg = EnResponseMessage.CreditWalletMsg;
                msg = msg.Replace("#Coin#", coinName);
                msg = msg.Replace("#TrnType#", routeTrnType.ToString());
                msg = msg.Replace("#TrnNo#", TrnRefNo.ToString());
                if (!string.IsNullOrEmpty(Token))
                {
                    HelperForLog.WriteLogIntoFile("GetWalletCreditNew Activity:With Token", "WalletService", "msg=" + msg + ",Token=" + Token + "WalletID" + walletMasterObj.AccWalletID + ",Balance" + walletMasterObj.Balance.ToString());
                    _signalRService.SendActivityNotification(msg, Token);
                    HelperForLog.WriteLogIntoFile("GetWalletCreditNew:With Token", "WalletService", "msg=" + msg + ",Token=" + Token + "WalletID" + walletMasterObj.AccWalletID + ",Balance" + walletMasterObj.Balance.ToString());
                    _signalRService.OnWalletBalChange(walletMasterObj, coinName, Token);
                }
                else
                {
                    HelperForLog.WriteLogIntoFile("GetWalletCreditNew Activity:Without Token", "WalletService", "msg=" + msg + ",Token=" + Token + "WalletID" + walletMasterObj.AccWalletID + ",Balance" + walletMasterObj.Balance.ToString());
                    _signalRService.SendActivityNotification(msg, cWalletobj.UserID.ToString(), 2);
                    HelperForLog.WriteLogIntoFile("GetWalletCreditNew:Without Token", "WalletService", "msg=" + msg + ",UserID =" + cWalletobj.UserID.ToString() + "WalletID" + walletMasterObj.AccWalletID + ",Balance" + walletMasterObj.Balance.ToString());
                    _signalRService.OnWalletBalChange(walletMasterObj, coinName, cWalletobj.UserID.ToString(), 2);
                }

                //-------------------------------

                return GetCrDRResponse(new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessCredit, ErrorCode = enErrorCode.Success, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg }, "GetWalletCreditNew");

                //tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 2);

                //for (int o = 0; o <= arryTrnID.Length - 1; o++)
                //{
                //    //woObj = InsertIntoWalletTransactionOrder(UTC_To_IST(), UTC_To_IST(), cWalletobj.Id, arryTrnID[0].dWalletId, arryTrnID[0].Amount, coinName, arryTrnID[0].DrTrnRefNo, tqObj.TrnNo, 1, "Updated");
                //    //woObj = _walletRepository1.AddIntoWalletTransactionOrder(woObj, 2);
                //}
                //return resp;

            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }


        public int CheckTrnRefNoForCredit(long TrnRefNo, enWalletTranxOrderType TrnType)
        {
            var count = _walletRepository1.CheckTrnRefNoForCredit(TrnRefNo, TrnType);
            return count;
        }

        public bool CheckarryTrnID(CreditWalletDrArryTrnID[] arryTrnID, string coinName)
        {
            bool obj = _walletRepository1.CheckarryTrnID(arryTrnID, coinName);
            return obj;
        }

        //Rushabh 15-10-2018 List All Addresses Of Specified Wallet
        public ListWalletAddressResponse ListAddress(string AccWalletID)
        {
            ListWalletAddressResponse AddressResponse = new ListWalletAddressResponse();
            try
            {
                var WalletAddResponse = _walletRepository1.ListAddressMasterResponse(AccWalletID);
                if (WalletAddResponse.Count == 0)
                {
                    AddressResponse.ReturnCode = enResponseCode.Fail;
                    AddressResponse.ReturnMsg = EnResponseMessage.NotFound;
                    AddressResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    AddressResponse.AddressList = WalletAddResponse;
                    AddressResponse.ReturnCode = enResponseCode.Success;
                    AddressResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    AddressResponse.ErrorCode = enErrorCode.Success;
                }
                return AddressResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                AddressResponse.ReturnCode = enResponseCode.InternalError;
                return AddressResponse;
            }
        }

        public ListWalletAddressResponse GetAddress(string AccWalletID)
        {
            ListWalletAddressResponse AddressResponse = new ListWalletAddressResponse();
            try
            {
                var WalletAddResponse = _walletRepository1.GetAddressMasterResponse(AccWalletID);
                if (WalletAddResponse.Count == 0)
                {
                    AddressResponse.ReturnCode = enResponseCode.Fail;
                    AddressResponse.ReturnMsg = EnResponseMessage.NotFound;
                    AddressResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    AddressResponse.AddressList = WalletAddResponse;
                    AddressResponse.ReturnCode = enResponseCode.Success;
                    AddressResponse.ErrorCode = enErrorCode.Success;
                    AddressResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    AddressResponse.ErrorCode = enErrorCode.Success;
                }
                return AddressResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                AddressResponse.ReturnCode = enResponseCode.InternalError;
                return AddressResponse;
            }
        }

        //16-10-2018 vsolanki 
        public DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            try
            {
                DateTime newTodate = ToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                DepositHistoryResponse response = _walletRepository1.DepositHistoy(FromDate, newTodate, Coin, Amount, Status, Userid);
                return response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //16-10-2018 vsolanki 
        public DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            try
            {
                DateTime NewTodate = ToDate.AddHours(23);
                NewTodate = NewTodate.AddMinutes(59);
                NewTodate = NewTodate.AddSeconds(59);
                DepositHistoryResponse response = _walletRepository1.WithdrawalHistoy(FromDate, NewTodate, Coin, Amount, Status, Userid);
                return response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public LimitResponse SetWalletLimitConfig(string accWalletID, WalletLimitConfigurationReq request, long Userid, string Token)
        {
            int type = Convert.ToInt16(request.TrnType);
            WalletLimitConfiguration IsExist = new WalletLimitConfiguration();
            WalletLimitConfigurationMaster MasterConfig = new WalletLimitConfigurationMaster();
            LimitResponse Response = new LimitResponse();
            try
            {
                //TimeSpan StartTime, EndTime;
                //System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0,System.DateTimeKind.Utc);
                //DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(dateTime1, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                //dateTime1 = istDate.AddSeconds(request.StartTime);
                //StartTime = dateTime1.TimeOfDay;
                //EndTime = dateTime1.TimeOfDay;

                //TimeSpan StartTime, EndTime;
                //System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                //DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(dateTime1, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                //StartTime = istDate.AddSeconds(request.StartTime).TimeOfDay;
                //EndTime = istDate.AddSeconds(request.EndTime).TimeOfDay;

                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == accWalletID);
                if (walletMasters == null)
                {
                    Response.ReturnCode = enResponseCode.Fail;
                    Response.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                IsExist = _LimitcommonRepository.GetSingle(item => item.TrnType == type && item.WalletId == walletMasters.Id);
                if (IsExist == null)
                {
                    WalletLimitConfiguration newobj = new WalletLimitConfiguration();
                    newobj.WalletId = walletMasters.Id;
                    newobj.TrnType = type;
                    newobj.LimitPerHour = request.LimitPerHour;
                    newobj.LimitPerDay = request.LimitPerDay;
                    newobj.LimitPerTransaction = request.LimitPerTransaction;
                    newobj.CreatedBy = Userid;
                    newobj.CreatedDate = UTC_To_IST();
                    newobj.Status = Convert.ToInt16(ServiceStatus.Active);
                    //obj.UpdatedDate = UTC_To_IST();
                    newobj.StartTimeUnix = request.StartTimeUnix;
                    newobj.LifeTime = request.LifeTime;
                    newobj.EndTimeUnix = request.EndTimeUnix;
                    newobj = _LimitcommonRepository.Add(newobj);
                    var msg = EnResponseMessage.CWalletLimitNotification;
                    msg = msg.Replace("#WalletName#", walletMasters.Walletname);
                    _signalRService.SendActivityNotification(msg, Token);
                    Response.ReturnMsg = EnResponseMessage.SetWalletLimitCreateMsg;
                    Response.ReturnCode = enResponseCode.Success;
                    Response.ErrorCode = enErrorCode.Success;
                }
                else
                {
                    MasterConfig = _WalletLimitConfigurationMasterRepository.GetSingle(item => item.TrnType == IsExist.TrnType);
                    if ((request.LimitPerDay <= MasterConfig.LimitPerDay) && (request.LimitPerHour <= MasterConfig.LimitPerHour) && (request.LimitPerTransaction <= MasterConfig.LimitPerTransaction))
                    {
                        IsExist.LimitPerHour = request.LimitPerHour;
                        IsExist.LimitPerDay = request.LimitPerDay;
                        IsExist.LifeTime = request.LifeTime;
                        IsExist.LimitPerTransaction = request.LimitPerTransaction;
                        IsExist.StartTimeUnix = request.StartTimeUnix;
                        IsExist.EndTimeUnix = request.EndTimeUnix;
                        IsExist.UpdatedBy = Userid;
                        IsExist.UpdatedDate = UTC_To_IST();
                        _LimitcommonRepository.Update(IsExist);
                        var msg = EnResponseMessage.UWalletLimitNotification;
                        msg = msg.Replace("#WalletName#", walletMasters.Walletname);
                        _signalRService.SendActivityNotification(msg, Token);
                        Response.ReturnMsg = EnResponseMessage.SetWalletLimitUpdateMsg;
                        Response.ReturnCode = enResponseCode.Success;
                        Response.ErrorCode = enErrorCode.Success;
                    }
                    else
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.InvalidLimit;
                        Response.ErrorCode = enErrorCode.InvalidLimit;
                    }
                }
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public LimitResponse GetWalletLimitConfig(string accWalletID)
        {
            LimitResponse LimitResponse = new LimitResponse();
            try
            {
                var WalletLimitResponse = _walletRepository1.GetWalletLimitResponse(accWalletID);
                if (WalletLimitResponse.Count == 0)
                {
                    LimitResponse.ReturnCode = enResponseCode.Fail;
                    LimitResponse.ReturnMsg = EnResponseMessage.NotFound;
                    LimitResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    //TimeSpan StartTime1, EndTime1;
                    //System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    //DateTime istDate = TimeZoneInfo.ConvertTimeFromUtc(dateTime1, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                    //var data = WalletLimitResponse.FirstOrDefault();
                    //StartTime1 = data.StartTime;
                    //WalletLimitConfigurationRes2 newRes = new WalletLimitConfigurationRes2();
                    //double stime = TimeSpan.Parse(data.StartTime).TotalSeconds;
                    //double etime = TimeSpan.Parse(data.EndTime).TotalSeconds;
                    //newRes.AccWalletID = data.AccWalletID;
                    //newRes.TrnType = data.TrnType;
                    //newRes.LimitPerDay = data.LimitPerDay;
                    //newRes.LimitPerHour = data.LimitPerHour;
                    //newRes.LimitPerTransaction = data.LimitPerTransaction;
                    //newRes.StartTime =
                    LimitResponse.WalletLimitConfigurationRes = WalletLimitResponse;
                    LimitResponse.ReturnCode = enResponseCode.Success;
                    LimitResponse.ErrorCode = enErrorCode.Success;
                    LimitResponse.ReturnMsg = EnResponseMessage.FindRecored;
                }
                return LimitResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                LimitResponse.ReturnCode = enResponseCode.InternalError;
                return LimitResponse;
            }
        }

        //vsolanki 18-10-2018
        public WithdrawalRes Withdrawl(string coin, string accWalletID, WithdrawalReq Request, long userid)
        {
            WalletTransactionQueue objTQ = new WalletTransactionQueue();
            try
            {
                var dWalletobj = _commonRepository.GetSingle(item => item.AccWalletID == accWalletID);
                if (dWalletobj == null)
                {
                    return new WithdrawalRes { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                }
                if (dWalletobj.Balance <= Request.amount)
                {
                    return new WithdrawalRes { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficientBal, ErrorCode = enErrorCode.InsufficantBal };
                }
                //objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), 0, Request.amount, 0, UTC_To_IST(), null, dWalletobj.Id, coin, userid, "", 0, "Inserted");
                //objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                // dWalletobj.DebitBalance(Request.amount);

                return new WithdrawalRes { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, txid = 12345 /*objTQ.TrnNo*/, statusMsg = "Success" /*objTQ.StatusMsg*/ };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetAvailableBalance(long userid, string walletId)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWalletId;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }
                var response = _walletRepository1.GetAvailableBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public TotalBalanceRes GetAllAvailableBalance(long userid)
        {
            TotalBalanceRes Response = new TotalBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetAllAvailableBalance(userid);
                decimal total = _walletRepository1.GetTotalAvailbleBal(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                Response.TotalBalance = total;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 24-10-2018
        public ListBalanceResponse GetUnSettledBalance(long userid, string walletId)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWalletId;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }
                var response = _walletRepository1.GetUnSettledBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetAllUnSettledBalance(long userid)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetAllUnSettledBalance(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetUnClearedBalance(long userid, string walletId)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWalletId;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }
                var response = _walletRepository1.GetUnClearedBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetAllUnClearedBalance(long userid)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetUnAllClearedBalance(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListStackingBalanceRes GetStackingBalance(long userid, string walletId)
        {
            ListStackingBalanceRes Response = new ListStackingBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWalletId;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }
                var response = _walletRepository1.GetStackingBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListStackingBalanceRes GetAllStackingBalance(long userid)
        {
            ListStackingBalanceRes Response = new ListStackingBalanceRes();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetAllStackingBalance(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetShadowBalance(long userid, string walletId)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWalletId;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }
                var response = _walletRepository1.GetShadowBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public ListBalanceResponse GetAllShadowBalance(long userid)
        {
            ListBalanceResponse Response = new ListBalanceResponse();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetAllShadowBalance(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }
        //vsolanki 24-10-2018
        public AllBalanceResponse GetAllBalances(long userid, string walletId)
        {

            AllBalanceResponse allBalanceResponse = new AllBalanceResponse();
            allBalanceResponse.BizResponseObj = new BizResponseClass();
            try
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == walletId);
                if (wallet == null)
                {
                    allBalanceResponse.BizResponseObj.ErrorCode = enErrorCode.InvalidWallet;
                    allBalanceResponse.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    allBalanceResponse.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return allBalanceResponse;
                }
                var response = _walletRepository1.GetAllBalances(userid, wallet.Id);
                if (response == null)
                {
                    allBalanceResponse.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    allBalanceResponse.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    allBalanceResponse.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return allBalanceResponse;
                }
                allBalanceResponse.BizResponseObj.ReturnCode = enResponseCode.Success;
                allBalanceResponse.BizResponseObj.ErrorCode = enErrorCode.Success;
                allBalanceResponse.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                allBalanceResponse.Balance = response;
                //vsolanki 2018-10-27 //for withdraw limit
                var limit = _LimitcommonRepository.GetSingle(item => item.TrnType == 2 && item.WalletId == wallet.Id);
                if (limit == null)
                {
                    allBalanceResponse.WithdrawalDailyLimit = 0;
                }
                if (limit.LimitPerDay < 0)
                {
                    allBalanceResponse.WithdrawalDailyLimit = 0;
                }
                else
                {
                    allBalanceResponse.WithdrawalDailyLimit = limit.LimitPerDay;

                }
                // var wallet = _commonRepository.GetById(walletId);
                var walletType = _WalletTypeMasterRepository.GetById(wallet.WalletTypeID);
                allBalanceResponse.WalletType = walletType.WalletTypeName;
                allBalanceResponse.WalletName = wallet.Walletname;
                allBalanceResponse.IsDefaultWallet = wallet.IsDefaultWallet;
                return allBalanceResponse;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BeneficiaryResponse AddBeneficiary(string CoinName, short WhitelistingBit, string Name, string BeneficiaryAddress, long UserId, string Token)
        {
            BeneficiaryMaster IsExist = new BeneficiaryMaster();
            BeneficiaryResponse Response = new BeneficiaryResponse();
            try
            {
                var userPreference = _UserPreferencescommonRepository.GetSingle(item => item.UserID == UserId);
                var walletMasters = _WalletTypeMasterRepository.GetSingle(item => item.WalletTypeName == CoinName);
                Response.BizResponse = new BizResponseClass();
                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                IsExist = _BeneficiarycommonRepository.GetSingle(item => item.Address == BeneficiaryAddress && item.WalletTypeID == walletMasters.Id && (item.Status == Convert.ToInt16(ServiceStatus.Active) || item.Status == Convert.ToInt16(ServiceStatus.InActive)));

                if (IsExist == null)
                {
                    BeneficiaryMaster AddNew = new BeneficiaryMaster();
                    if (userPreference.IsWhitelisting == 1)
                    {
                        AddNew.IsWhiteListed = 1;
                    }
                    else
                    {
                        AddNew.IsWhiteListed = 0;
                    }
                    AddNew.Status = Convert.ToInt16(ServiceStatus.Active);
                    AddNew.CreatedBy = UserId;
                    AddNew.CreatedDate = UTC_To_IST();
                    AddNew.UserID = UserId;
                    AddNew.Address = BeneficiaryAddress;
                    AddNew.Name = Name;
                    AddNew.IsWhiteListed = WhitelistingBit;
                    AddNew.WalletTypeID = walletMasters.Id;
                    AddNew = _BeneficiarycommonRepository.Add(AddNew);
                    var msg = EnResponseMessage.AddBeneNotification;
                    msg = msg.Replace("#WalletName#", walletMasters.WalletTypeName);
                    _signalRService.SendActivityNotification(msg, Token);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.RecordAdded;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                }
                else
                {
                    Response.BizResponse.ReturnMsg = EnResponseMessage.AlredyExist;
                    Response.BizResponse.ErrorCode = enErrorCode.AlredyExist;
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                }
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BeneficiaryResponse ListWhitelistedBeneficiary(string AccWalletID, long UserId)
        {
            //BeneficiaryMaster IsExist = new BeneficiaryMaster();
            BeneficiaryResponse Response = new BeneficiaryResponse();
            Response.BizResponse = new BizResponseClass();
            try
            {
                //var userPreference = _UserPreferencescommonRepository.GetSingle(item => item.UserID == UserId);
                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID && item.UserID == UserId && item.Status == Convert.ToInt16(ServiceStatus.Active));

                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                var BeneficiaryMasterRes = _walletRepository1.GetAllWhitelistedBeneficiaries(walletMasters.WalletTypeID, walletMasters.UserID);
                if (BeneficiaryMasterRes.Count == 0)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.NotFound;
                    Response.BizResponse.ErrorCode = enErrorCode.NotFound;
                    return Response;
                }
                else
                {
                    Response.Beneficiaries = BeneficiaryMasterRes;
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    return Response;
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BeneficiaryResponse ListBeneficiary(long UserId)
        {
            BeneficiaryResponse Response = new BeneficiaryResponse();
            Response.BizResponse = new BizResponseClass();
            try
            {
                //var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID && item.UserID == UserId && item.Status == 1);

                //if (walletMasters == null)
                //{
                //    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                //    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                //    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                //    return Response;
                //}
                var BeneficiaryMasterRes = _walletRepository1.GetAllBeneficiaries(UserId);
                if (BeneficiaryMasterRes.Count == 0)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.NotFound;
                    Response.BizResponse.ErrorCode = enErrorCode.NotFound;
                    return Response;
                }
                else
                {
                    Response.Beneficiaries = BeneficiaryMasterRes;
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    return Response;
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 25-10-2018
        public BalanceResponseWithLimit GetAvailbleBalTypeWise(long userid)
        {
            BalanceResponseWithLimit Response = new BalanceResponseWithLimit();
            Response.BizResponseObj = new Core.ApiModels.BizResponseClass();
            try
            {
                var response = _walletRepository1.GetAvailbleBalTypeWise(userid);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                decimal total = _walletRepository1.GetTotalAvailbleBal(userid);
                //vsolanki 26-10-2018
                var walletType = _WalletTypeMasterRepository.GetSingle(item => item.IsDefaultWallet == 1);
                if (walletType == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidCoinName;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidCoin;
                    return Response;
                }
                var wallet = _commonRepository.GetSingle(item => item.IsDefaultWallet == 1 && item.WalletTypeID == walletType.Id);
                if (wallet == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWallet;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                    return Response;
                }

                var limit = _LimitcommonRepository.GetSingle(item => item.TrnType == 9 && item.WalletId == wallet.Id);//for withdraw

                if (limit == null)
                {
                    Response.DailyLimit = 0;

                }
                else
                {
                    Response.DailyLimit = limit.LimitPerDay;

                }
                //get amt from  tq
                var amt = _walletRepository1.GetTodayAmountOfTQ(userid, wallet.Id);

                if (response.Count == 0)
                {
                    Response.UsedLimit = 0;

                }
                else
                {
                    Response.UsedLimit = amt;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                Response.Response = response;
                Response.TotalBalance = total;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsolanki 25-10-2018
        public ListAllBalanceTypeWiseRes GetAllBalancesTypeWise(long userId, string WalletType)
        {
            ListAllBalanceTypeWiseRes res = new ListAllBalanceTypeWiseRes();
            AllBalanceTypeWiseRes a = new AllBalanceTypeWiseRes();
            List<AllBalanceTypeWiseRes> Response = new List<AllBalanceTypeWiseRes>();
            res.BizResponseObj = new Core.ApiModels.BizResponseClass();
            a.Wallet = new WalletResponse();
            a.Wallet.Balance = new Balance();
            var listWallet = _walletRepository1.GetWalletMasterResponseByCoin(userId, WalletType);
            if (listWallet.Count() == 0)
            {
                res.BizResponseObj.ReturnCode = enResponseCode.Fail;
                res.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                res.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                return res;
            }
            for (int i = 0; i <= listWallet.Count - 1; i++)
            {
                var wallet = _commonRepository.GetSingle(item => item.AccWalletID == listWallet[i].AccWalletID);
                var response = _walletRepository1.GetAllBalances(userId, wallet.Id);

                a.Wallet.AccWalletID = listWallet[i].AccWalletID;
                a.Wallet.PublicAddress = listWallet[i].PublicAddress;
                a.Wallet.WalletName = listWallet[i].WalletName;
                a.Wallet.IsDefaultWallet = listWallet[i].IsDefaultWallet;
                a.Wallet.TypeName = listWallet[i].CoinName;

                a.Wallet.Balance = response;
                Response.Add(a);
            }
            if (Response.Count() == 0)
            {
                res.BizResponseObj.ReturnCode = enResponseCode.Fail;
                res.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                res.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                return res;
            }
            res.Wallets = Response;
            res.BizResponseObj.ReturnCode = enResponseCode.Success;
            res.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
            res.BizResponseObj.ErrorCode = enErrorCode.Success;
            return res;
        }

        public UserPreferencesRes SetPreferences(long Userid, int GlobalBit, string Token)
        {
            UserPreferencesRes Response = new UserPreferencesRes();
            Response.BizResponse = new BizResponseClass();
            try
            {
                UserPreferencesMaster IsExist = _UserPreferencescommonRepository.GetSingle(item => item.UserID == Userid);
                if (IsExist == null)
                {
                    UserPreferencesMaster newobj = new UserPreferencesMaster();
                    newobj.UserID = Userid;
                    newobj.IsWhitelisting = Convert.ToInt16(GlobalBit);
                    newobj.CreatedBy = Userid;
                    newobj.CreatedDate = UTC_To_IST();
                    newobj.Status = Convert.ToInt16(ServiceStatus.Active);
                    newobj = _UserPreferencescommonRepository.Add(newobj);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.SetUserPrefSuccessMsg;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                }
                else
                {
                    IsExist.IsWhitelisting = Convert.ToInt16(GlobalBit);
                    IsExist.UpdatedBy = Userid;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _UserPreferencescommonRepository.Update(IsExist);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.SetUserPrefUpdateMsg;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                }
                Response.BizResponse.ReturnCode = enResponseCode.Success;
                var msg = EnResponseMessage.UserPreferencesNotification;
                if (GlobalBit == 1)
                {
                    msg = msg.Replace("#ONOFF#", "ON");
                }
                else
                {
                    msg = msg.Replace("#ONOFF#", "OFF");
                }
                _signalRService.SendActivityNotification(msg, Token);
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public UserPreferencesRes GetPreferences(long Userid)
        {
            UserPreferencesRes Response = new UserPreferencesRes();
            Response.BizResponse = new BizResponseClass();
            try
            {
                UserPreferencesMaster IsExist = _UserPreferencescommonRepository.GetSingle(item => item.UserID == Userid);
                if (IsExist == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.NotFound;
                    Response.BizResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    Response.IsWhitelisting = IsExist.IsWhitelisting;
                    Response.UserID = IsExist.UserID;
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                }
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BeneficiaryResponse UpdateBulkBeneficiary(BulkBeneUpdateReq Request, long ID, string Token)
        {
            BeneficiaryResponse Response = new BeneficiaryResponse();
            Response.BizResponse = new BizResponseClass();
            try
            {
                bool state = _walletRepository1.BeneficiaryBulkEdit(Request);
                if (state == true)
                {
                    var msg = EnResponseMessage.UpBeneNotification;
                    //msg = msg.Replace("#WalletName#", walletMasters.WalletTypeName);
                    _signalRService.SendActivityNotification(msg, Token);
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.RecordUpdated;
                }
                else
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.NotFound;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidBeneficiaryID;
                }
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BeneficiaryResponse UpdateBeneficiaryDetails(BeneficiaryUpdateReq request, string AccWalletID, long UserID, string Token)
        {
            BeneficiaryResponse Response = new BeneficiaryResponse();
            BeneficiaryMaster IsExist = new BeneficiaryMaster();
            Response.BizResponse = new BizResponseClass();
            try
            {
                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID);
                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                IsExist = _BeneficiarycommonRepository.GetSingle(item => item.Id == request.BenefifiaryID && item.WalletTypeID == walletMasters.WalletTypeID && item.UserID == UserID);
                if (IsExist != null)
                {
                    IsExist.Name = request.Name;
                    IsExist.Status = Convert.ToInt16(request.Status);
                    IsExist.IsWhiteListed = Convert.ToInt16(request.WhitelistingBit);
                    IsExist.UpdatedBy = UserID;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _BeneficiarycommonRepository.Update(IsExist);
                    var msg = EnResponseMessage.UpBeneNotification;
                    msg = msg.Replace("#WalletName#", walletMasters.Walletname);
                    _signalRService.SendActivityNotification(msg, Token);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.RecordUpdated;
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
                    Response.BizResponse.ErrorCode = enErrorCode.Success;
                }
                else
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.NotFound;
                    Response.BizResponse.ErrorCode = enErrorCode.NotFound;
                }
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        //vsoalnki 26-10-2018
        public ListWalletLedgerRes GetWalletLedger(DateTime FromDate, DateTime ToDate, string WalletId, int page)
        {
            var wallet = _commonRepository.GetSingle(item => item.AccWalletID == WalletId);

            ListWalletLedgerRes Response = new ListWalletLedgerRes();
            Response.BizResponseObj = new BizResponseClass();
            if (wallet == null)
            {
                Response.BizResponseObj.ErrorCode = enErrorCode.InvalidWallet;
                Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidWallet;
                return Response;
            }
            DateTime newToDate = ToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
            var wl = _walletRepository1.GetWalletLedger(FromDate, newToDate, wallet.Id, page);
            if (wl.Count() == 0)
            {
                Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                return Response;
            }
            Response.WalletLedgers = wl;
            Response.BizResponseObj.ReturnCode = enResponseCode.Success;
            Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
            Response.BizResponseObj.ErrorCode = enErrorCode.Success;
            return Response;
        }


        //vsolanki 27-10-2018
        public BizResponseClass CreateDefaulWallet(long UserID, string accessToken = null)
        {

            var res = _walletRepository1.CreateDefaulWallet(UserID);
            if (res != 1)
            {
                return new BizResponseClass
                {
                    ErrorCode = enErrorCode.InternalError,
                    ReturnMsg = EnResponseMessage.CreateWalletFailMsg,
                    ReturnCode = enResponseCode.InternalError
                };
            }
            AddBizUserTypeMappingReq req = new AddBizUserTypeMappingReq();
            req.UserID = UserID;
            req.UserType = enUserType.User;
            AddBizUserTypeMapping(req);
            if (accessToken != null)
            {
                var msg = EnResponseMessage.DefaultCreateWalletSuccessMsg;
                //msg = msg.Replace("#WalletName#", walletMaster.Walletname);
                _signalRService.SendActivityNotification(msg, accessToken);
            }
            return new BizResponseClass
            {
                ErrorCode = enErrorCode.Success,
                ReturnMsg = EnResponseMessage.CreateWalletSuccessMsg,
                ReturnCode = enResponseCode.Success
            };
        }

        //vsolanki 27-10-2018
        public BizResponseClass CreateWalletForAllUser_NewService(string WalletType)
        {
            var walletType = _WalletTypeMasterRepository.GetSingle(item => item.WalletTypeName == WalletType);
            if (walletType == null)
            {
                return new BizResponseClass
                {
                    ErrorCode = enErrorCode.InvalidCoinName,
                    ReturnMsg = EnResponseMessage.InvalidCoin,
                    ReturnCode = enResponseCode.Fail
                };
            }
            //var wallet = _commonRepository.GetSingle(item=>item.WalletTypeID== walletType.Id && item.IsDefaultWallet==1);
            var res = _walletRepository1.CreateWalletForAllUser_NewService(WalletType);
            if (res != 1)
            {
                return new BizResponseClass
                {
                    ErrorCode = enErrorCode.InternalError,
                    ReturnMsg = EnResponseMessage.CreateWalletFailMsg,
                    ReturnCode = enResponseCode.InternalError
                };
            }
            return new BizResponseClass
            {
                ErrorCode = enErrorCode.Success,
                ReturnMsg = EnResponseMessage.CreateWalletSuccessMsg,
                ReturnCode = enResponseCode.Success
            };
        }

        //vsolanki 2018-10-29
        public BizResponseClass AddBizUserTypeMapping(AddBizUserTypeMappingReq req)
        {
            BizUserTypeMapping bizUser = new BizUserTypeMapping();
            bizUser.UserID = req.UserID;
            bizUser.UserType = Convert.ToInt16(req.UserType);
            var res = _walletRepository1.AddBizUserTypeMapping(bizUser);
            if (res == 0)
            {
                return new BizResponseClass
                {
                    ErrorCode = enErrorCode.DuplicateRecord,
                    ReturnMsg = EnResponseMessage.DuplicateRecord,
                    ReturnCode = enResponseCode.Fail
                };
            }
            return new BizResponseClass
            {
                ErrorCode = enErrorCode.Success,
                ReturnMsg = EnResponseMessage.RecordAdded,
                ReturnCode = enResponseCode.Success
            };
        }

        //vsolanki 2018-10-29
        public ListIncomingTrnRes GetIncomingTransaction(long Userid, string Coin)
        {
            try
            {
                ListIncomingTrnRes Response = new ListIncomingTrnRes();
                Response.BizResponseObj = new BizResponseClass();
                var depositHistories = _walletRepository1.GetIncomingTransaction(Userid, Coin);
                if (depositHistories.Count() == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.IncomingTransactions = depositHistories;
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

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
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
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
                if (total == wObjBal && total > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
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

        //vsoalnki 2018-10-31
        public CreateWalletAddressRes CreateETHAddress(string Coin, int AddressCount, long UserId, string token)
        {
            try
            {
                CreateWalletAddressRes addr = new CreateWalletAddressRes();
                var orgid = _walletRepository1.getOrgID();
                if (orgid != UserId)
                {
                    return new CreateWalletAddressRes { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.OrgIDNotFound, ErrorCode = enErrorCode.OrgIDNotFound };
                }
                var type = _WalletTypeMasterRepository.GetSingle(t => t.WalletTypeName == Coin);
                if (type == null)
                {
                    return new CreateWalletAddressRes { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidCoin, ErrorCode = enErrorCode.InvalidCoinName };
                }
                var walletObj = _commonRepository.FindBy(t => t.UserID == orgid && t.IsDefaultWallet == 1 && t.WalletTypeID == type.Id).FirstOrDefault();
                if (walletObj == null)
                {
                    return new CreateWalletAddressRes { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                }
                for (int i = 1; i <= AddressCount; i++)
                {
                    addr = GenerateAddress(walletObj.AccWalletID, Coin, token, 1);
                    if (addr.address == null)
                    {
                        return addr;
                    }
                }
                return addr;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
                //return false;
            }
        }

        //vsolanki 2018-11-02
        public ListOutgoingTrnRes GetOutGoingTransaction(long Userid, string Coin)
        {
            try
            {
                ListOutgoingTrnRes Response = new ListOutgoingTrnRes();
                Response.BizResponseObj = new BizResponseClass();
                var type = _WalletTypeMasterRepository.GetSingle(i => i.WalletTypeName == Coin);
                if (Coin != null)
                {
                    if (type == null)
                    {
                        Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                        Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidCoin;
                        Response.BizResponseObj.ErrorCode = enErrorCode.InvalidCoinName;
                        return Response;
                    }
                }
                var Histories = _walletRepository1.GetOutGoingTransaction(Userid, Coin);
                if (Histories.Count() == 0 || Histories == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.OutGoingTransactions = Histories;
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
                //return false;
            }

        }

        //vsolanki 2018-11-03
        public ListTokenConvertHistoryRes ConvertFundHistory(long Userid, DateTime FromDate, DateTime ToDate, string Coin)
        {
            try
            {
                ListTokenConvertHistoryRes Response = new ListTokenConvertHistoryRes();
                Response.BizResponseObj = new BizResponseClass();
                if (Coin != null)
                {
                    var type = _WalletTypeMasterRepository.GetSingle(i => i.WalletTypeName == Coin);
                    if (type == null)
                    {
                        Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                        Response.BizResponseObj.ReturnMsg = EnResponseMessage.InvalidCoin;
                        Response.BizResponseObj.ErrorCode = enErrorCode.InvalidCoinName;
                        return Response;
                    }
                }
                DateTime newTodate = ToDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                var Histories = _walletRepository1.ConvertFundHistory(Userid, FromDate, newTodate, Coin);
                if (Histories.Count() == 0 || Histories == null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.HistoryRes = Histories;
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.BizResponseObj.ErrorCode = enErrorCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }

        }

        //vsolanki 2018-11-03
        public decimal ConvertFund(decimal SourcePrice)
        {
            try
            {
                if (SourcePrice > 0)
                {
                    decimal total = (SourcePrice * 10) / 100;
                    return total;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public BizResponseClass AddIntoConvertFund(ConvertTockenReq Request, long userid, string accessToken = null)
        {
            try
            {
                ConvertFundHistory h = new ConvertFundHistory();
                h.CreatedBy = userid;
                h.CreatedDate = UTC_To_IST();
                h.UpdatedBy = userid;
                h.UpdatedDate = UTC_To_IST();
                h.Status = Convert.ToInt16(ServiceStatus.InActive);
                h.SourcePrice = Request.SourcePrice;
                h.DestinationPrice = Request.DestinationPrice;
                h.FromWalletId = Request.SourceWalletId;
                h.ToWalletId = Request.DestinationWalletId;
                h.Price = 10;
                h.TrnDate = UTC_To_IST();
                _ConvertFundHistory.Add(h);
                if (accessToken != null)
                {
                    var msg = EnResponseMessage.ConvertFund;
                    msg = msg.Replace("#SourcePrice#", h.SourcePrice.ToString());
                    msg = msg.Replace("#DestinationPrice#", h.DestinationPrice.ToString());
                    _signalRService.SendActivityNotification(msg, accessToken);
                }
                return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.RecordAdded, ErrorCode = enErrorCode.Success };
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                throw ex;
            }
        }

        public bool InsertIntoWithdrawHistory(WithdrawHistory req)
        {
            try
            {
                if (req != null)
                {
                    _WithdrawHistoryRepository.Add(req);
                    return true;
                }
                else
                {
                    return false;
                }
                //WithdrawHistory withdrawHistory = new WithdrawHistory();
                //withdrawHistory.CreatedDate = UTC_To_IST();
                //withdrawHistory.CreatedBy = req.UserId;
                //withdrawHistory.UpdatedBy = req.UserId;
                //withdrawHistory.UpdatedDate = null;
                //withdrawHistory.Status = req.Status;
                //withdrawHistory.SMSCode = req.SMSCode;
                //withdrawHistory.Wallet = req.Wallet;
                //withdrawHistory.Address = req.Address;
                //withdrawHistory.Confirmations = req.Confirmations;
                //withdrawHistory.Value = req.Value;
                //withdrawHistory.Amount = req.Amount;
                //withdrawHistory.Charge = req.Charge;
                //withdrawHistory.State = req.State;
                //withdrawHistory.confirmedTime = req.confirmedTime;
                //withdrawHistory.unconfirmedTime = req.unconfirmedTime;
                //withdrawHistory.createdTime = req.createdTime;
                //withdrawHistory.IsProcessing = req.IsProcessing;
                //withdrawHistory.ToAddress = req.ToAddress;
                //withdrawHistory.APITopUpRefNo = req.APITopUpRefNo;
                //withdrawHistory.SystemRemarks = req.SystemRemarks;
                //withdrawHistory.TrnNo = req.TrnNo;
                //withdrawHistory.RouteTag = req.RouteTag;
                //withdrawHistory.SerProID = req.SerProID;
                //withdrawHistory.UserId = req.UserId;
                //withdrawHistory.TrnDate = UTC_To_IST();


                //return true;
            }
            catch (Exception ex)
            {
                HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                //throw ex;
                return false;
            }
        }

        public WalletDrCrResponse GetCrDRResponse(WalletDrCrResponse obj, string extras)
        {
            try
            {
                HelperForLog.WriteLogIntoFile(extras, "WalletService", "ReturnCode=" + obj.ReturnCode + ",ReturnMsg=" + obj.ReturnMsg + ",StatusMsg=" + obj.StatusMsg + ",TrnNo=" + obj.TrnNo);
                return obj;
            }
            catch (Exception ex)
            {
                //HelperForLog.WriteErrorLog(System.Reflection.MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex);
                return obj;
            }
        }
    }

}