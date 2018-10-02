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

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : BasePage, IWalletService 
    {
        readonly ILogger<WalletService> _log;
        readonly ICommonRepository<WalletMaster> _commonRepository;
        readonly ICommonRepository<WalletOrder> _walletOrderRepository;
        readonly ICommonRepository<TrnAcBatch> _trnBatch;
        //readonly ICommonRepository<WalletLedger> _walletLedgerRepository;
        readonly IWalletRepository _walletRepository1;

        //readonly IBasePage _BaseObj;

        public WalletService(ILogger<WalletService> log, ICommonRepository<WalletMaster> commonRepository,
            ICommonRepository<TrnAcBatch> BatchLogger, ICommonRepository<WalletOrder> walletOrderRepository,ILogger<BasePage> logger) : base(logger)
        {
            _log = log;
            _commonRepository = commonRepository;
            _walletOrderRepository = walletOrderRepository;
            //_walletRepository = repository;
            //_walletOrderRepository = walletOrderRepository;           
            _trnBatch = BatchLogger;
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
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    response.ErrorCode = 7001;
                    response.ReturnMsg = "Invalid Account";
                    response.ReturnCode = 1;
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
                response.ErrorCode = 7002;
                response.ReturnMsg = "Successfully Inserted";
                response.ReturnCode = 0;
                return response;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
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
                    bizResponse.StatusCode = 1;
                    return bizResponse;
                }
                if (balance < amount)
                    {
                    // return false;
                    bizResponse.ErrorCode = enErrorCode.InsufficientBalance;
                    bizResponse.ReturnMsg = "Insufficient Balance";
                    bizResponse.StatusCode = 1;
                    return bizResponse;
                }
                    var dWalletobj = _commonRepository.GetById(DWalletID);
                    if (dWalletobj == null || dWalletobj.Status != 1)
                    {
                    // return false;
                    bizResponse.ErrorCode = enErrorCode.InvalidWallet;
                    bizResponse.ReturnMsg = "Invalid Wallet";
                    bizResponse.StatusCode = 1;
                    return bizResponse;
                }
                    var oWalletobj = _commonRepository.GetById(OWalletID);
                    if (oWalletobj == null || oWalletobj.Status != 1)
                    {
                    bizResponse.ErrorCode = enErrorCode.InvalidWallet;
                    bizResponse.ReturnMsg = "Invalid Wallet";
                    bizResponse.StatusCode = 1;
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
                bizResponse.StatusCode = 0;
                return bizResponse;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

    }
}
