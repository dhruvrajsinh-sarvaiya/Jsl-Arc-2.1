using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Infrastructure.Interfaces;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Data;

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : BasePage, IWalletService 
    {
        readonly ILogger<WalletService> _log;
        readonly IWalletRepository<WalletMaster> _walletRepository;
        readonly IWalletRepository<WalletOrder> _walletOrderRepository;
        readonly IWalletRepository<WalletOrder> _walletOrder;
        //readonly IBasePage _BaseObj;

        public WalletService(ILogger<WalletService> log, IWalletRepository<WalletMaster> repository, IWalletRepository<WalletOrder> walletOrderRepository,ILogger<BasePage> logger) : base(logger)
        {
            _log = log;
            _walletRepository = repository;
            _walletOrderRepository = walletOrderRepository;
            //_BaseObj = basePage;
        }

        public bool CreditWallet(long walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }

        public bool DebitWallet(long walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }
        
        public decimal GetUserBalance(long walletId)
        {
            try
            {
                var obj = _walletRepository.GetById(walletId);
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
                return _walletRepository.GetById(walletId).IsValid;
                
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
                var obj =_walletRepository.GetById(walletid);
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
      
    }
}
