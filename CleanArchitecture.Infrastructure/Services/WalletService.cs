using System;
using System.Collections.Generic;
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

namespace CleanArchitecture.Infrastructure.Services
{
    public class WalletService : BasePage, IWalletService
    {
        private readonly ILogger<WalletService> _log;
        private readonly ICommonRepository<WalletMaster> _commonRepository;
        private readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyCommonRepository;
        private readonly ICommonRepository<WalletOrder> _walletOrderRepository;
        private readonly ICommonRepository<AddressMaster> _addressMstRepository;
        private readonly ICommonRepository<TrnAcBatch> _trnBatch;
        private readonly ICommonRepository<TradeBitGoDelayAddresses> _bitgoDelayRepository;
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepo;

        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        private readonly IWalletRepository _walletRepository1;
        private readonly IWebApiRepository _webApiRepository;
        private readonly IWebApiSendRequest _webApiSendRequest;
        private readonly IGetWebRequest _getWebRequest;
        private readonly WebApiParseResponse _WebApiParseResponse;

        //vsolanki 8-10-2018 
        private readonly ICommonRepository<WalletTypeMaster> _WalletTypeMasterRepository;
        //readonly IBasePage _BaseObj;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        //vsolanki 10-10-2018 
        private readonly ICommonRepository<WalletAllowTrn> _WalletAllowTrnRepository;

        public WalletService(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository, WebApiParseResponse WebApiParseResponse,
            ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,
            IWebApiRepository webApiRepository, IWebApiSendRequest webApiSendRequest, ICommonRepository<ThirdPartyAPIConfiguration> thirdpartyCommonRepo,
            IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
            ILogger<BasePage> logger, ICommonRepository<WalletTypeMaster> WalletTypeMasterRepository, ICommonRepository<WalletAllowTrn> WalletAllowTrnRepository,
            ICommonRepository<WalletAllowTrn> WalletAllowTrnRepo) : base(logger)
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

        public bool WalletBalanceCheck(decimal amount, long walletid)
        {
            try
            {
                var obj = _commonRepository.GetById(walletid);
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

        public CreateWalletAddressRes GenerateAddress(long walletID, string coin)
        {
            try
            {
                ThirdPartyAPIRequest thirdPartyAPIRequest;
                //WebApiParseResponse _WebApiParseResponse;
                TradeBitGoDelayAddresses delayAddressesObj, delayGeneratedAddressesObj;
                List<TransactionProviderResponse> transactionProviderResponses;
                WalletMaster walletMaster = _commonRepository.GetById(walletID);
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
                thirdPartyAPIRequest = _getWebRequest.MakeWebRequest(transactionProviderResponses[0].RouteID, transactionProviderResponses[0].ThirPartyAPIID, transactionProviderResponses[0].ServiceProID);
                string apiResponse = _webApiSendRequest.SendAPIRequestAsync(thirdPartyAPIRequest.RequestURL, thirdPartyAPIRequest.RequestBody, thirdPartyAPIConfiguration.ContentType, 180000, thirdPartyAPIRequest.keyValuePairsHeader, thirdPartyAPIConfiguration.MethodType);
                // parse response logic 
                if (string.IsNullOrEmpty(apiResponse) && thirdPartyAPIRequest.DelayAddress == 1)
                {
                    delayAddressesObj = GetTradeBitGoDelayAddresses(walletID, walletMaster.WalletTypeID, TrnID, address, thirdPartyAPIRequest.walletID, walletMaster.CreatedBy, CoinSpecific, 0, 0);
                    delayAddressesObj = _bitgoDelayRepository.Add(delayAddressesObj);
                    delayGeneratedAddressesObj = _walletRepository1.GetUnassignedETH();
                    if (delayGeneratedAddressesObj != null)
                    {
                        address = delayGeneratedAddressesObj.Address;
                        delayGeneratedAddressesObj.WalletId = walletID;
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
                    addressMaster = GetAddressObj(walletID, transactionProviderResponses[0].ServiceProID, Respaddress, "Self Address", walletMaster.UserID, 0, 1);
                    addressMaster = _addressMstRepository.Add(addressMaster);
                    string responseString = Respaddress;
                    //CreateWalletAddressRes Response = new CreateWalletAddressRes();
                    //Response = JsonConvert.DeserializeObject<CreateWalletAddressRes>(responseString);
                    //Response.ReturnCode = enResponseCode.Success;
                    //var respObj = JsonConvert.SerializeObject(Response);
                    //dynamic respObjJson = JObject.Parse(respObj);
                    return new CreateWalletAddressRes { address = Respaddress, ErrorCode = enErrorCode.Success, ReturnCode = enResponseCode.Success, ReturnMsg = EnResponseMessage.CreateWalletSuccessMsg };
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
        public CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId)
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
                var addressClass = GenerateAddress(walletMaster.Id, CoinName);
                //walletMaster.PublicAddress = addressClass.address;
                walletMaster.WalletPublicAddress(addressClass.address);              
                _commonRepository.Update(walletMaster);

                //set the response object value
                createWalletResponse.AccWalletID = walletMaster.AccWalletID;
                createWalletResponse.PublicAddress = walletMaster.PublicAddress;
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
                if (walletResponse == null)
                {
                    listWalletResponse.ReturnCode = enResponseCode.Fail;
                    listWalletResponse.ReturnMsg = EnResponseMessage.NotFound;
                    listWalletResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    listWalletResponse.wallets = walletResponse;
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
                if (walletResponse == null)
                {
                    listWalletResponse.ReturnCode = enResponseCode.Fail;
                    listWalletResponse.ReturnMsg = EnResponseMessage.NotFound;
                    listWalletResponse.ErrorCode = enErrorCode.NotFound;
                }
                else
                {
                    listWalletResponse.wallets = walletResponse;
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
    }
}
