using CleanArchitecture.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.ApiModels;

namespace CleanArchitecture.Infrastructure.Services
{
    class WalletService : IWalletService
    {
        readonly ILogger<WalletService> _log;
        readonly IWalletRepository<WalletMaster> _walletRepository;
        readonly IWalletRepository<WalletOrder> _walletOrderRepository;

        readonly IWalletRepository<WalletOrder> _walletOrder;


        public WalletService(ILogger<WalletService> log, IWalletRepository<WalletMaster> repository, IWalletRepository<WalletOrder> walletOrderRepository)
        {
            _log = log;
            _walletRepository = repository;
            _walletOrderRepository = walletOrderRepository;
        }

        public bool CreditWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }

        public bool DebitWallet(int walletId, ref decimal PostBal)
        {
            throw new NotImplementedException();
        }
        
        public decimal GetUserBalance(int walletId)
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
                var orderItem = new WalletOrder()
                {
                    CreatedBy = 1, // temperory bind member not now
                    DeliveryAmt = Order.OrderAmt,
                    OrderDate = DateTime.UtcNow,
                    OrderType = Order.OrderType,
                    OrderAmt= Order.OrderAmt,
                    DWalletMasterID = Order.DWalletMasterID,
                    OWalletMasterID = Order.OWalletMasterID,
                    Status = 0,
                    CreatedDate = DateTime.UtcNow,
                    ORemarks = Order.ORemarks
                };
                _walletOrderRepository.Add(orderItem);
                response.OrderID = orderItem.Id;
                response.ORemarks = "Successfully Inserted";
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
