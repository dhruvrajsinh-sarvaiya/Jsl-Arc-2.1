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

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : BasePage, IWalletService 
    {
        readonly ILogger<WalletService> _log;
        readonly ICommonRepository<WalletMaster> _commonRepository;
        readonly ICommonRepository<ThirdPartyAPIConfiguration> _thirdPartyCommonRepository;      
        readonly ICommonRepository<WalletOrder> _walletOrderRepository;
        readonly ICommonRepository<AddressMaster> _addressMstRepository;
        readonly ICommonRepository<TrnAcBatch> _trnBatch;
        readonly ICommonRepository<TradeBitGoDelayAddresses> _bitgoDelayRepository;
        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        readonly IWalletRepository _walletRepository1;
        readonly IWebApiRepository _webApiRepository ;
        readonly IWebApiSendRequest _webApiSendRequest;
        readonly IGetWebRequest _getWebRequest;

        //readonly IBasePage _BaseObj;

        public WalletService(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository,
            ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository, IWalletRepository walletRepository,
            IWebApiRepository webApiRepository, IWebApiSendRequest webApiSendRequest, ICommonRepository<ThirdPartyAPIConfiguration> thirdpartyCommonRepo,
            IGetWebRequest getWebRequest, ICommonRepository<TradeBitGoDelayAddresses> bitgoDelayRepository, ICommonRepository<AddressMaster> addressMaster,
            ILogger<BasePage> logger) : base(logger)
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
            //_walletLedgerRepository = walletledgerrepo;
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

        public bool WalletBalanceCheck(decimal amount,long walletid)
        {
            try
            {
                var obj = _commonRepository.GetById(walletid);
                if(obj.Balance < amount)
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
        public CreateOrderResponse CreateOrder (CreateOrderRequest Order)
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
                    OrderAmt= Order.OrderAmt,
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


        public BizResponse ProcessOrder(long RefNo,long DWalletID,long OWalletID,decimal amount,string remarks, enTrnType enTrnType,enServiceType serviceType)
        {
            try
            {
                TransactionAccount tansAccObj = new TransactionAccount();
                TransactionAccount tansAccObj1 = new TransactionAccount();
                BizResponse bizResponse = new BizResponse();


                decimal balance;                
               
                    balance = GetUserBalance(DWalletID);
               if(amount <  0)
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
                    walletLedger.TrnType = enTrnType.Deposit;
                    walletLedger.CrAmt = 0;
                    walletLedger.CreatedBy = DWalletID;
                    walletLedger.CreatedDate = UTC_To_IST();
                    walletLedger.DrAmt = amount;                   
                    walletLedger.TrnNo = RefNo;
                    walletLedger.Remarks = remarks;
                    walletLedger.Status = 1;
                    walletLedger.TrnDate = UTC_To_IST();
                    walletLedger.UpdatedBy = DWalletID;
                    walletLedger.WalletMasterId = DWalletID;
                    walletLedger.ToWalletMasterId = OWalletID;
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
                    walletLedger2.WalletMasterId = OWalletID;
                    walletLedger2.ToWalletMasterId = DWalletID;
                    walletLedger2.PreBal = oWalletobj.Balance;                 
                    walletLedger2.PostBal = oWalletobj.Balance - amount;

                    _walletRepository1.WalletOperation(walletLedger,walletLedger2,tansAccObj,tansAccObj1, dWalletobj, oWalletobj);
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


        public  BizResponse GenerateAddress(long walletID)
        {
            try
            {
                ThirdPartyAPIRequest thirdPartyAPIRequest;
                TradeBitGoDelayAddresses delayAddressesObj, delayGeneratedAddressesObj;
                List<TransactionProviderResponse> transactionProviderResponses;
                WalletMaster walletMaster = _commonRepository.GetById(walletID);
                AddressMaster addressMaster;
                string address="";
                string CoinSpecific = null;
                string TrnID = null;
                               
                if (walletMaster == null)
                {
                    //return false
                    return new BizResponse {ErrorCode = enErrorCode.InvalidWallet , ReturnCode =  enResponseCodeService.Fail, ReturnMsg = "Invalid Amount" };
                }
                else if(walletMaster.Status != 1)
                {
                    //return false
                    return new BizResponse { ErrorCode = enErrorCode.InvalidWallet, ReturnCode = enResponseCodeService.Fail, ReturnMsg = "Invalid Amount" };
                }

                transactionProviderResponses = _webApiRepository.GetProviderDataList( new TransactionApiConfigurationRequest { amount = 0,SMSCode = walletMaster.CoinName , APIType = enWebAPIRouteType.TransactionAPI , trnType = enTrnType.Generate_Address });
                if(transactionProviderResponses == null )
                {
                    return new BizResponse { ErrorCode = enErrorCode.ItemNotFoundForGenerateAddress, ReturnCode = enResponseCodeService.Fail, ReturnMsg = "Please try after sometime." };                    
                }
                if (transactionProviderResponses[0].ThirPartyAPIID == 0)
                {
                    return new BizResponse { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCodeService.Fail, ReturnMsg = "Please try after sometime." };
                }

                ThirdPartyAPIConfiguration thirdPartyAPIConfiguration = _thirdPartyCommonRepository.GetById(transactionProviderResponses[0].ThirPartyAPIID);
                if (thirdPartyAPIConfiguration == null)
                {
                    return new BizResponse { ErrorCode = enErrorCode.InvalidThirdpartyID, ReturnCode = enResponseCodeService.Fail, ReturnMsg = "Please try after sometime." };
                }
                thirdPartyAPIRequest =_getWebRequest.MakeWebRequest(transactionProviderResponses[0].RouteID, transactionProviderResponses[0].ThirPartyAPIID, transactionProviderResponses[0].SerProID);
                string apiResponse =_webApiSendRequest.SendAPIRequestAsync(thirdPartyAPIRequest.RequestURL, thirdPartyAPIRequest.RequestBody, thirdPartyAPIConfiguration.ContentType, 60,thirdPartyAPIRequest.keyValuePairsHeader, thirdPartyAPIConfiguration.MethodType);
                // parse response logic need to be implemented
                if(string.IsNullOrEmpty(apiResponse) && thirdPartyAPIRequest.DelayAddress == 1)
                {
                    delayAddressesObj = GetTradeBitGoDelayAddresses(walletID,walletMaster.WalletTypeID,TrnID,address, walletMaster.CoinName, thirdPartyAPIRequest.walletID ,walletMaster.CreatedBy,CoinSpecific,0,0);
                    delayAddressesObj = _bitgoDelayRepository.Add(delayAddressesObj);
                    delayGeneratedAddressesObj = _walletRepository1.GetUnassignedETH();
                    if(delayGeneratedAddressesObj != null)
                    {
                        address = delayGeneratedAddressesObj.Address;
                        delayGeneratedAddressesObj.WalletId = walletID;
                        delayGeneratedAddressesObj.UpdatedBy = walletMaster.UserID;
                        delayGeneratedAddressesObj.UpdatedDate = UTC_To_IST();
                        _bitgoDelayRepository.Update(delayGeneratedAddressesObj);
                    }
                }
                if(!string.IsNullOrEmpty(address))
                {
                    addressMaster = GetAddressObj(walletID, transactionProviderResponses[0].SerProID, address, walletMaster.CoinName, "Self Address", walletMaster.UserID, 0, 1);
                    addressMaster = _addressMstRepository.Add(addressMaster);
                    return new BizResponse { ErrorCode = enErrorCode.Success, ReturnCode = enResponseCodeService.Success, ReturnMsg = "Success." };
                }
                else
                {
                    return new BizResponse { ErrorCode = enErrorCode.AddressGenerationFailed, ReturnCode = enResponseCodeService.Fail, ReturnMsg = "Failed to generate Address." };
                }

                // code need to be added
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Date: "+ UTC_To_IST() +",\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public AddressMaster GetAddressObj(long walletID,long serproID, string address,string coinName,string addressName,long createdBy, byte isDefaultAdd,short status)
        {
            try
            {
                AddressMaster addressMaster = new AddressMaster();
                addressMaster.Address = address;
                addressMaster.CoinName = coinName;
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


        public TradeBitGoDelayAddresses GetTradeBitGoDelayAddresses(long walletID, long WalletTypeId, string TrnID,string address, string coinName, string BitgoWalletId, long createdBy, string CoinSpecific, short status,byte generatebit)
        {
            try
            {
                TradeBitGoDelayAddresses addressMaster = new TradeBitGoDelayAddresses
                {
                    CoinSpecific = CoinSpecific,
                    Address = address,
                    BitgoWalletId = BitgoWalletId,
                    CoinName = coinName,
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


        public WalletMaster GetWalletMaster(long WalletTypeId, string walletName, bool isValid, short status , long createdBy,string coinname)
        {
            try
            {
                WalletMaster addressMaster = new WalletMaster
                {                   
                    CoinName = coinname,
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
       
    }
}
