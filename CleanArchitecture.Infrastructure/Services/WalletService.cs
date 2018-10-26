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

        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        private readonly IWalletRepository _walletRepository1;
        private readonly IWebApiRepository _webApiRepository;
        private readonly IWebApiSendRequest _webApiSendRequest;
        private readonly IGetWebRequest _getWebRequest;
        private readonly WebApiParseResponse _WebApiParseResponse;

        //vsolanki 8-10-2018 
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        //  private readonly ICommonRepository<HistoryObject> _DepositHistoryRepository;
        //readonly IBasePage _BaseObj;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        //vsolanki 10-10-2018 
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepository;

        //private readonly IRepository<WalletTransactionOrder> _WalletAllowTrnRepository;
        //  private readonly ICommonRepository<WalletTransactionQueue> t;

        public WalletService(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository, WebApiParseResponse WebApiParseResponse,
            ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,
            IWebApiRepository webApiRepository, IWebApiSendRequest webApiSendRequest, ICommonRepository<ThirdPartyAPIConfiguration> thirdpartyCommonRepo,
            IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
            ILogger<BasePage> logger, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ICommonRepository<WalletAllowTrn> WalletAllowTrnRepository,
            ICommonRepository<WalletAllowTrn> WalletAllowTrnRepo, ICommonRepository<WalletLimitConfigurationMaster> WalletConfigMasterRepo , ICommonRepository<BeneficiaryMaster> BeneficiaryMasterRepo, ICommonRepository<UserPreferencesMaster> UserPreferenceRepo, ICommonRepository<WalletLimitConfiguration> WalletLimitConfig) : base(logger)
        {
            _log = log;
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool WalletBalanceCheck(decimal amount, string walletid)
        {
            try
            {
                var obj = _commonRepository.GetSingle(item=>item.AccWalletID== walletid);
                if (obj.Balance < amount)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public CreateWalletAddressRes GenerateAddress(string walletID, string coin)
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

                if (walletMaster == null)
                {
                    //return false enResponseCodeService.Fail
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }
                else if (walletMaster.Status != 1)
                {
                    //return false
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet };
                }

                transactionProviderResponses = _webApiRepository.GetProviderDataList(new TransactionApiConfigurationRequest { SMSCode = coin.ToLower(), amount = 0, APIType = enWebAPIRouteType.TransactionAPI, trnType = Convert.ToInt32(enTrnType.Generate_Address) });
                if (transactionProviderResponses == null)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress, ReturnCode = enResponseCode.Fail, ReturnMsg = "Please try after sometime." };
                }
                if (transactionProviderResponses[0].ThirPartyAPIID == 0)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ItemOrThirdprtyNotFound };
                }

                ThirdPartyAPIConfiguration thirdPartyAPIConfiguration = _thirdPartyCommonRepository.GetById(transactionProviderResponses[0].ThirPartyAPIID);
                if (thirdPartyAPIConfiguration == null)
                {
                    return new CreateWalletAddressRes { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.ItemOrThirdprtyNotFound };
                }
                thirdPartyAPIRequest = _getWebRequest.MakeWebRequest(transactionProviderResponses[0].RouteID, transactionProviderResponses[0].ThirPartyAPIID, transactionProviderResponses[0].SerProDetailID);
                string apiResponse = _webApiSendRequest.SendAPIRequestAsync(thirdPartyAPIRequest.RequestURL, thirdPartyAPIRequest.RequestBody, thirdPartyAPIConfiguration.ContentType, 180000, thirdPartyAPIRequest.keyValuePairsHeader, thirdPartyAPIConfiguration.MethodType);
                // parse response logic 
                if (string.IsNullOrEmpty(apiResponse) && thirdPartyAPIRequest.DelayAddress == 1)
                {
                    delayAddressesObj = GetTradeBitGoDelayAddresses(walletMaster.Id, walletMaster.WalletTypeID, TrnID, address, thirdPartyAPIRequest.walletID, walletMaster.CreatedBy, CoinSpecific, 0, 0);
                    delayAddressesObj = _bitgoDelayRepository.Add(delayAddressesObj);
                    delayGeneratedAddressesObj = _walletRepository1.GetUnassignedETH();
                    if (delayGeneratedAddressesObj != null)
                    {
                        address = delayGeneratedAddressesObj.Address;
                        delayGeneratedAddressesObj.WalletId = walletMaster.Id;
                        delayGeneratedAddressesObj.UpdatedBy = walletMaster.UserID;
                        delayGeneratedAddressesObj.UpdatedDate = UTC_To_IST();
                        _bitgoDelayRepository.Update(delayGeneratedAddressesObj);
                    }
                }
                if (!string.IsNullOrEmpty(apiResponse))
                {
                    WebAPIParseResponseCls ParsedResponse = _WebApiParseResponse.TransactionParseResponse(apiResponse, transactionProviderResponses[0].ThirPartyAPIID);
                    Respaddress = ParsedResponse.TrnRefNo;
                }

                if (!string.IsNullOrEmpty(Respaddress))
                {
                    addressMaster = GetAddressObj(walletMaster.Id, transactionProviderResponses[0].ServiceProID, Respaddress, "Self Address", walletMaster.UserID, 0, 1);
                    addressMaster = _addressMstRepository.Add(addressMaster);
                    string responseString = Respaddress;
                    //CreateWalletAddressRes Response = new CreateWalletAddressRes();
                    //Response = JsonConvert.DeserializeObject<CreateWalletAddressRes>(responseString);
                    //Response.ReturnCode = enResponseCode.Success;
                    //var respObj = JsonConvert.SerializeObject(Response);
                    //dynamic respObjJson = JObject.Parse(respObj);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public TradeBitGoDelayAddresses GetTradeBitGoDelayAddresses(long walletID, long WalletTypeId, string TrnID, string address, string BitgoWalletId, long createdBy, string CoinSpecific, short status, byte generatebit)
        {
            try
            {
                TradeBitGoDelayAddresses addressMaster = new TradeBitGoDelayAddresses
                {
                    CoinSpecific = CoinSpecific,
                    Address = address,
                    BitgoWalletId = BitgoWalletId,
                    //oinName = coinName,
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        //vsolanki 10-10-2018 Insert into WalletMaster table
        public CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId,int isBaseService = 0)
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
                    var addressClass = GenerateAddress(walletMaster.AccWalletID, CoinName);
                    //walletMaster.PublicAddress = addressClass.address;
                    walletMaster.WalletPublicAddress(addressClass.address);
                    _commonRepository.Update(walletMaster);
                }
                //vsolanki 26-10-2018 insert and limitConfigration
                _walletRepository1.GetSetLimitConfigurationMaster(AllowTrnType,userId,walletMaster.Id);

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
                return createWalletResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                return new BizResponseClass { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CommSuccessMsgInternal };

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    walletLedger2.PostBal = currentBalance + drAmount;
                }
                return walletLedger2;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                }
                return listWalletResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
            long WalletID, string WalletType, long MemberID, string TimeStamp, enTransactionStatus Status, string StatusMsg,enWalletTrnType enWalletTrnType)
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

        public WalletDrCrResponse GetWalletDeductionNew(string coinName, string timestamp, enWalletTranxOrderType orderType, decimal amount, long userID, string accWalletID, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType)
        {
            try
            {
                WalletMaster dWalletobj;
                string remarks = "";
                WalletTypeMaster walletTypeMaster;
                WalletTransactionQueue objTQ;
                //long walletTypeID;
                WalletDrCrResponse resp = new WalletDrCrResponse();
                if (string.IsNullOrEmpty(accWalletID) || coinName == string.Empty || userID == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidWalletOrUserIDorCoinName };
                }
                walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                if (walletTypeMaster == null)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName };
                }
                dWalletobj = _commonRepository.GetSingle(e => e.UserID == userID && e.WalletTypeID == walletTypeMaster.Id && e.AccWalletID == accWalletID);
                if (dWalletobj == null)
                {
                    //tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid().ToString(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 2, EnResponseMessage.InvalidWallet);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                }
                if (dWalletobj.Status != 1 || dWalletobj.IsValid == false)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidWallet, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (orderType != enWalletTranxOrderType.Debit) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTrnType, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTrnType, ErrorCode = enErrorCode.InvalidTrnType, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (TrnRefNo == 0) // sell 13-10-2018
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (amount <= 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidAmt, ErrorCode = enErrorCode.InvalidAmount, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                if (dWalletobj.Balance < amount)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InsufficantBal, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InsufficantBal, ErrorCode = enErrorCode.InsufficantBal, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }
                int count = CheckTrnRefNo(TrnRefNo, orderType);
                if (count != 0)
                {
                    // insert with status=2 system failed
                    objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.AlredyExist, trnType);
                    objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };
                }

                objTQ = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, amount, TrnRefNo, UTC_To_IST(), null, dWalletobj.Id, coinName, userID, timestamp, 0, "Inserted", trnType);
                objTQ = _walletRepository1.AddIntoWalletTransactionQueue(objTQ, 1);
                TrnAcBatch batchObj = _trnBatch.Add(new TrnAcBatch(UTC_To_IST()));
                remarks = "Debit for TrnNo:" + objTQ.TrnNo;
                WalletLedger walletLedger = GetWalletLedger(dWalletobj.Id, 0, amount, 0, trnType, serviceType, objTQ.TrnNo, remarks, dWalletobj.Balance, 1);
                TransactionAccount tranxAccount = GetTransactionAccount(dWalletobj.Id, 1, batchObj.Id, amount, 0, objTQ.TrnNo, remarks, 1);
                dWalletobj.DebitBalance(amount);
                objTQ.Status = enTransactionStatus.Hold;
                objTQ.StatusMsg = "Hold";
                _walletRepository1.WalletDeductionwithTQ(walletLedger, tranxAccount, dWalletobj, objTQ);
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessDebit, ErrorCode = enErrorCode.Success, TrnNo = objTQ.TrnNo, Status = objTQ.Status, StatusMsg = objTQ.StatusMsg };

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }


        public int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType)
        {
            var count = _walletRepository1.CheckTrnRefNo(TrnRefNo, TrnType);
            return count;
        }

        public WalletDrCrResponse GetWalletCreditNew(string coinName, string timestamp, enWalletTrnType trnType, decimal TotalAmount, long userID, string crAccWalletID, CreditWalletDrArryTrnID[] arryTrnID, long TrnRefNo, short isFullSettled, enWalletTranxOrderType orderType, enServiceType serviceType)
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

                if (string.IsNullOrEmpty(crAccWalletID) || coinName == string.Empty || userID == 0)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidWalletOrUserIDorCoinName };
                }
                walletTypeMaster = _WalletTypeMasterRepository.GetSingle(e => e.WalletTypeName == coinName);
                if (walletTypeMaster == null)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.InvalidCoinName };
                }
                cWalletobj = _commonRepository.GetSingle(e => e.UserID == userID && e.WalletTypeID == walletTypeMaster.Id && e.AccWalletID == crAccWalletID);
                if (cWalletobj == null)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidReq, ErrorCode = enErrorCode.UserIDWalletIDDidNotMatch };
                }
                if (cWalletobj.Status != 1 || cWalletobj.IsValid == false)
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidWallet, ErrorCode = enErrorCode.InvalidWallet };
                }
                if (orderType != enWalletTranxOrderType.Credit) // buy 
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTrnType, ErrorCode = enErrorCode.InvalidTrnType };
                }

                //WalletTransactionQueue
                if (TrnRefNo == 0) // buy
                {
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo };
                }
                if (TotalAmount <= 0)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidAmt, trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg };
                }
                int count = CheckTrnRefNoForCredit(TrnRefNo, enWalletTranxOrderType.Debit); // check whether for this refno wallet is pre decuted or not
                if (count == 0)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, EnResponseMessage.InvalidTradeRefNo,trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);
                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.AlredyExist, ErrorCode = enErrorCode.AlredyExist };
                }
                bool checkArray = CheckarryTrnID(arryTrnID, coinName);// check whether all array dtrnrefno of same wallet and they are hold (not assigned to other order)
                if (checkArray == true)
                {
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, 0, "Inserted",trnType);
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
                    tqObj = InsertIntoWalletTransactionQueue(Guid.NewGuid(), orderType, TotalAmount, TrnRefNo, UTC_To_IST(), null, cWalletobj.Id, coinName, userID, timestamp, enTransactionStatus.SystemFail, "Amount and DebitRefNo matching failure",trnType);
                    tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 1);

                    return new WalletDrCrResponse { ReturnCode = enResponseCode.Fail, ReturnMsg = EnResponseMessage.InvalidTradeRefNo, ErrorCode = enErrorCode.InvalidTradeRefNo, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg };
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
                return new WalletDrCrResponse { ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.SuccessCredit, ErrorCode = enErrorCode.Success, TrnNo = tqObj.TrnNo, Status = tqObj.Status, StatusMsg = tqObj.StatusMsg };

                //tqObj = _walletRepository1.AddIntoWalletTransactionQueue(tqObj, 2);

                //for (int o = 0; o <= arryTrnID.Length - 1; o++)
                //{
                //    //woObj = InsertIntoWalletTransactionOrder(UTC_To_IST(), UTC_To_IST(), cWalletobj.Id, arryTrnID[0].dWalletId, arryTrnID[0].Amount, coinName, arryTrnID[0].DrTrnRefNo, tqObj.TrnNo, 1, "Updated");
                //    //woObj = _walletRepository1.AddIntoWalletTransactionOrder(woObj, 2);
                //}
                return resp;

            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                }
                return AddressResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    AddressResponse.ReturnMsg = EnResponseMessage.FindRecored;
                }
                return AddressResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                AddressResponse.ReturnCode = enResponseCode.InternalError;
                return AddressResponse;
            }
        }

        //16-10-2018 vsolanki 
        public DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            try
            {
                DepositHistoryResponse response = _walletRepository1.DepositHistoy(FromDate, ToDate, Coin, Amount, Status, Userid);
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        //16-10-2018 vsolanki 
        public DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            try
            {
                DepositHistoryResponse response = _walletRepository1.WithdrawalHistoy(FromDate, ToDate, Coin, Amount, Status, Userid);
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public LimitResponse SetWalletLimitConfig(string accWalletID, WalletLimitConfigurationReq request, long Userid)
        {
            int type = Convert.ToInt16(request.TrnType);
            WalletLimitConfiguration IsExist = new WalletLimitConfiguration();
            WalletLimitConfigurationMaster MasterConfig = new WalletLimitConfigurationMaster();
            LimitResponse Response = new LimitResponse();
            try
            {
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
                    newobj.StartTime = request.StartTime;
                    newobj.EndTime = request.EndTime;
                    newobj = _LimitcommonRepository.Add(newobj);
                    Response.ReturnMsg = EnResponseMessage.SetWalletLimitCreateMsg;
                   // Response.WalletLimitConfigurationRes = newobj;
                }
                else
                {
                    MasterConfig = _WalletLimitConfigurationMasterRepository.GetSingle(item => item.TrnType == IsExist.TrnType);
                    if((request.LimitPerDay <= MasterConfig.LimitPerDay) && (request.LimitPerHour <= MasterConfig.LimitPerHour) && (request.LimitPerTransaction <= MasterConfig.LimitPerTransaction))
                    {
                        IsExist.LimitPerHour = request.LimitPerHour;
                        IsExist.LimitPerDay = request.LimitPerDay;
                        IsExist.LimitPerTransaction = request.LimitPerTransaction;
                        IsExist.StartTime = request.StartTime;
                        IsExist.EndTime = request.EndTime;
                        IsExist.UpdatedBy = Userid;
                        IsExist.UpdatedDate = UTC_To_IST();
                        _LimitcommonRepository.Update(IsExist);
                        Response.ReturnMsg = EnResponseMessage.SetWalletLimitUpdateMsg;
                    }
                    else
                    {
                        Response.ReturnCode = enResponseCode.Fail;
                        Response.ReturnMsg = EnResponseMessage.InvalidLimit;
                        Response.ErrorCode = enErrorCode.InvalidLimit;
                    }
                }
                Response.ReturnCode = enResponseCode.Success;                
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    LimitResponse.WalletLimitConfigurationRes = WalletLimitResponse;
                    LimitResponse.ReturnCode = enResponseCode.Success;
                    LimitResponse.ReturnMsg = EnResponseMessage.FindRecored;
                }
                return LimitResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                var response = _walletRepository1.GetAvailableBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                Response.TotalBalance = total;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                var response = _walletRepository1.GetUnSettledBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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

                var response = _walletRepository1.GetStackingBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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

                var response = _walletRepository1.GetShadowBalance(userid, wallet.Id);
                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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

                var response = _walletRepository1.GetAllBalances(userid, wallet.Id);
                if (response == null)
                {
                    allBalanceResponse.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    allBalanceResponse.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    allBalanceResponse.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return allBalanceResponse;
                }
                allBalanceResponse.BizResponseObj.ReturnCode = enResponseCode.Success;
                allBalanceResponse.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                allBalanceResponse.Balance = response;
               // var wallet = _commonRepository.GetById(walletId);
                var walletType = _WalletTypeMasterRepository.GetById(wallet.WalletTypeID);
                allBalanceResponse.WalletType = walletType.WalletTypeName;
                allBalanceResponse.WalletName = wallet.Walletname;
                allBalanceResponse.IsDefaultWallet = wallet.IsDefaultWallet;
                return allBalanceResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public BeneficiaryResponse AddBeneficiary(string AccWalletID, string BeneficiaryAddress, long UserId)
        {
            BeneficiaryMaster IsExist = new BeneficiaryMaster();
            BeneficiaryResponse Response = new BeneficiaryResponse();
            try
            {
                var userPreference = _UserPreferencescommonRepository.GetSingle(item => item.UserID == UserId);
                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID);
                Response.BizResponse = new BizResponseClass();
                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                IsExist = _BeneficiarycommonRepository.GetSingle(item => item.Address == BeneficiaryAddress && item.WalletTypeID == walletMasters.WalletTypeID && item.Status == 1);

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
                    AddNew.Status = 1;
                    AddNew.CreatedBy = UserId;
                    AddNew.CreatedDate = UTC_To_IST();
                    AddNew.UserID = UserId;
                    AddNew.Address = BeneficiaryAddress;
                    AddNew.Name = "System Generated";
                    AddNew.WalletTypeID = walletMasters.WalletTypeID;
                    AddNew = _BeneficiarycommonRepository.Add(AddNew);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.RecordAdded;
                }
                else
                {
                    Response.BizResponse.ReturnMsg = EnResponseMessage.AlredyExist;
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                }
                Response.BizResponse.ReturnCode = enResponseCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID && item.UserID == UserId && item.Status == 1);

                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                var BeneficiaryMasterRes = _walletRepository1.GetAllWhitelistedBeneficiaries(walletMasters.WalletTypeID);
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
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    return Response;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public BeneficiaryResponse ListBeneficiary(string AccWalletID, long UserId)
        {
            BeneficiaryResponse Response = new BeneficiaryResponse();
            Response.BizResponse = new BizResponseClass();
            try
            {
                var walletMasters = _commonRepository.GetSingle(item => item.AccWalletID == AccWalletID && item.UserID == UserId && item.Status == 1);

                if (walletMasters == null)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Fail;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.InvalidWallet;
                    Response.BizResponse.ErrorCode = enErrorCode.InvalidWalletId;
                    return Response;
                }
                var BeneficiaryMasterRes = _walletRepository1.GetAllBeneficiaries(walletMasters.WalletTypeID);
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
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                    return Response;
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                decimal total = _walletRepository1.GetTotalAvailbleBal(userid);

                //vsolanki 26-10-2018
                var walletType = _WalletTypeMasterRepository.GetSingle(item => item.IsDefaultWallet==1);
                if (walletType==null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                var wallet = _commonRepository.GetSingle(item => item.IsDefaultWallet == 1 && item.WalletTypeID== walletType.Id);
                if (wallet==null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }

                var limit = _LimitcommonRepository.GetSingle(item=>item.TrnType==9 && item.WalletId== wallet.Id);//for withdraw

                if (limit==null)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                //get amt from  tq
                var amt = _walletRepository1.GetTodayAmountOfTQ(userid, wallet.Id);

                if (response.Count == 0)
                {
                    Response.BizResponseObj.ErrorCode = enErrorCode.NotFound;
                    Response.BizResponseObj.ReturnCode = enResponseCode.Fail;
                    Response.BizResponseObj.ReturnMsg = EnResponseMessage.NotFound;
                    return Response;
                }
                Response.BizResponseObj.ReturnCode = enResponseCode.Success;
                Response.BizResponseObj.ReturnMsg = EnResponseMessage.FindRecored;
                Response.Response = response;
                Response.DailyLimit = limit.LimitPerDay;
                Response.UsedLimit = amt;
                Response.TotalBalance = total;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        //vsolanki 25-10-2018
        public List<AllBalanceTypeWiseRes> GetAllBalancesTypeWise(long userId, string WalletType)
        {
            AllBalanceTypeWiseRes a = new AllBalanceTypeWiseRes();
            List<AllBalanceTypeWiseRes> Response = new List<AllBalanceTypeWiseRes>();
            a.Wallet = new WalletResponse();
            a.Wallet.Balance = new Balance();
            var listWallet = _walletRepository1.GetWalletMasterResponseByCoin(userId, WalletType);
            for (int i = 0; i <= listWallet.Count-1; i++)
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
            return Response;
        }

        public UserPreferencesRes SetPreferences(long Userid,int GlobalBit)
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
                }
                else
                {
                    IsExist.IsWhitelisting = Convert.ToInt16(GlobalBit);
                    IsExist.UpdatedBy = Userid;
                    IsExist.UpdatedDate = UTC_To_IST();
                    _UserPreferencescommonRepository.Update(IsExist);
                    Response.BizResponse.ReturnMsg = EnResponseMessage.SetUserPrefUpdateMsg;
                }
                Response.BizResponse.ReturnCode = enResponseCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    Response.UserID  = IsExist.UserID;
                    Response.BizResponse.ReturnMsg = EnResponseMessage.FindRecored;
                }
                Response.BizResponse.ReturnCode = enResponseCode.Success;
                return Response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public BeneficiaryResponse UpdateBulkBeneficiary(BulkBeneUpdateReq[] Request, long ID)
        {
            BeneficiaryResponse Response = new BeneficiaryResponse();
            Response.BizResponse = new BizResponseClass();
            try
            {
                bool state = _walletRepository1.BeneficiaryBulkEdit(Request);
                if (state == true)
                {
                    Response.BizResponse.ReturnCode = enResponseCode.Success;
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public BeneficiaryResponse UpdateBeneficiaryDetails(BeneficiaryUpdateReq request,string AccWalletID, long UserID)
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
                    Response.BizResponse.ReturnMsg = EnResponseMessage.RecordUpdated;
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
                _log.LogError(ex, "Date: " + UTC_To_IST() + ",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        //vsolanki 26-10-2018
        public void CreateDefaulWallet(long UserID)
        {

        }
    }

}