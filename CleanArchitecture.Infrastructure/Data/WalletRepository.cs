using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Helpers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using CleanArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace CleanArchitecture.Infrastructure.Data
{
    public class WalletRepository : IWalletRepository
    {
        private readonly CleanArchitectureContext _dbContext;

        private readonly ILogger<WalletRepository> _log;

        public WalletRepository(ILogger<WalletRepository> log, CleanArchitectureContext dbContext)
        {
            _log = log;
            _dbContext = dbContext;
        }

        //public T GetById(long id)
        //{
        //    try
        //    {
        //        return _dbContext.Set<T>().SingleOrDefault(e => e.Id == id);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        //public List<T> List()
        //{
        //    try
        //    {                
        //        return _dbContext.Set<T>().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        //public T Add(T entity)
        //{
        //    try
        //    {
        //        _dbContext.Set<T>().Add(entity);
        //        _dbContext.SaveChanges();

        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        //public void Delete(T entity)
        //{
        //    try
        //    {
        //        _dbContext.Set<T>().Remove(entity);
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        //public void Update(T entity)
        //{
        //    try
        //    {
        //        _dbContext.Entry(entity).State = EntityState.Modified;
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        //public T AddProduct(T entity)
        //{
        //    try
        //    {
        //        _dbContext.Set<T>().Add(entity);
        //        _dbContext.SaveChanges();

        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
        //        throw ex;
        //    }
        //}

        public TradeBitGoDelayAddresses GetUnassignedETH()
        {
            try
            { // returns the address for ETH which are previously generated but not assinged to any wallet ntrivedi 26-09-2018
                return _dbContext.Set<TradeBitGoDelayAddresses>().Where(e => e.GenerateBit == 1 && e.WalletId == 0).OrderBy(e => e.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public bool WalletOperation(WalletLedger wl1, WalletLedger wl2, TransactionAccount ta1, TransactionAccount ta2, WalletMaster wm2, WalletMaster wm1)
        {
            try
            { // returns the address for ETH which are previously generated but not assinged to any wallet ntrivedi 26-09-2018

                _dbContext.Database.BeginTransaction();
                _dbContext.Set<WalletLedger>().Add(wl1);
                _dbContext.Set<WalletLedger>().Add(wl2);
                _dbContext.Set<TransactionAccount>().Add(ta1);
                _dbContext.Set<TransactionAccount>().Add(ta2);
                _dbContext.Entry(wm1).State = EntityState.Modified;
                _dbContext.Entry(wm2).State = EntityState.Modified;
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool WalletDeduction(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2)
        {
            try
            { // returns the address for ETH which are previously generated but not assinged to any wallet ntrivedi 26-09-2018

                _dbContext.Database.BeginTransaction();
                _dbContext.Set<WalletLedger>().Add(wl1);
                _dbContext.Set<TransactionAccount>().Add(ta1);
                _dbContext.Entry(wm2).State = EntityState.Modified;
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }

        public bool WalletDeductionwithTQ(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2, WalletTransactionQueue wtq)
        {
            try
            { // returns the address for ETH which are previously generated but not assinged to any wallet ntrivedi 26-09-2018

                _dbContext.Database.BeginTransaction();
                //_dbContext.Set<WalletTransactionQueue>().Add(wtq);
                //wl1.TrnNo = wtq.TrnNo;
                _dbContext.Set<WalletLedger>().Add(wl1);
                _dbContext.Set<TransactionAccount>().Add(ta1);
                _dbContext.Entry(wm2).State = EntityState.Modified;
                _dbContext.Entry(wtq).State = EntityState.Modified;
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }


        public List<WalletMasterResponse> ListWalletMasterResponse(long UserId)
        {
            List<WalletMasterResponse> items = (from u in _dbContext.WalletMasters
                                                join c in _dbContext.WalletTypeMasters
                                                       on u.WalletTypeID equals c.Id
                                                where u.UserID == UserId
                                                select new WalletMasterResponse
                                                {
                                                    AccWalletID = u.AccWalletID,
                                                    WalletName = u.Walletname,
                                                    CoinName = c.WalletTypeName,
                                                    PublicAddress = u.PublicAddress,
                                                    Balance = u.Balance,
                                                    IsDefaultWallet = u.IsDefaultWallet
                                                }).AsEnumerable().ToList();
            return items;
        }

        public List<WalletMasterResponse> GetWalletMasterResponseByCoin(long UserId, string coin)
        {
            List<WalletMasterResponse> items = (from u in _dbContext.WalletMasters
                                                join c in _dbContext.WalletTypeMasters
                                                       on u.WalletTypeID equals c.Id
                                                where u.UserID == UserId && c.WalletTypeName == coin
                                                select new WalletMasterResponse
                                                {
                                                    AccWalletID = u.AccWalletID,
                                                    WalletName = u.Walletname,
                                                    CoinName = c.WalletTypeName,
                                                    PublicAddress = u.PublicAddress,
                                                    Balance = u.Balance,
                                                    IsDefaultWallet = u.IsDefaultWallet
                                                }).AsEnumerable().ToList();
            return items;
        }

        public List<WalletMasterResponse> GetWalletMasterResponseById(long UserId, string coin, string walletId)
        {
            List<WalletMasterResponse> items = (from u in _dbContext.WalletMasters
                                                join c in _dbContext.WalletTypeMasters
                                                       on u.WalletTypeID equals c.Id
                                                where u.UserID == UserId && c.WalletTypeName == coin && u.AccWalletID == walletId
                                                select new WalletMasterResponse
                                                {
                                                    AccWalletID = u.AccWalletID,
                                                    WalletName = u.Walletname,
                                                    CoinName = c.WalletTypeName,
                                                    PublicAddress = u.PublicAddress,
                                                    Balance = u.Balance,
                                                    IsDefaultWallet = u.IsDefaultWallet
                                                }).AsEnumerable().ToList();
            return items;
        }


        public int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType)
        {
            int response = (from u in _dbContext.WalletTransactionQueues
                            where u.TrnRefNo == TrnRefNo && u.TrnType == TrnType
                            select u).Count();
            return response;
        }

        public int CheckTrnRefNoForCredit(long TrnRefNo, enWalletTranxOrderType TrnType) // need to check whether walleet is pre deducted for this order
        {
            int response = (from u in _dbContext.WalletTransactionQueues
                            where u.TrnRefNo == TrnRefNo && u.TrnType == TrnType && (u.Status == enTransactionStatus.Hold || u.Status == enTransactionStatus.Success)
                            select u).Count();
            return response;
        }

        public WalletTransactionQueue AddIntoWalletTransactionQueue(WalletTransactionQueue wtq, byte AddorUpdate)//1=add,2=update
        {
            //WalletTransactionQueue w = new WalletTransactionQueue();
            if (AddorUpdate == 1)
            {
                _dbContext.WalletTransactionQueues.Add(wtq);
            }
            else
            {
                _dbContext.Entry(wtq).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
            return wtq;
        }
        public WalletTransactionOrder AddIntoWalletTransactionOrder(WalletTransactionOrder wo, byte AddorUpdate)//1=add,2=update)
        {
            if (AddorUpdate == 1)
            {
                _dbContext.WalletTransactionOrders.Add(wo);
            }
            else
            {
                _dbContext.Entry(wo).State = EntityState.Modified;
            }
            _dbContext.SaveChanges();
            return wo;
        }

        public bool CheckarryTrnID(CreditWalletDrArryTrnID[] arryTrnID, string coinName)
        {
            bool i = false;
            decimal totalAmtDrTranx;
            for (int t = 0; t <= arryTrnID.Length - 1; t++)
            {
                var response = (from u in _dbContext.WalletTransactionQueues
                                where u.TrnRefNo == arryTrnID[t].DrTrnRefNo && u.Status == enTransactionStatus.Hold && u.TrnType == Core.Enums.enWalletTranxOrderType.Debit
                                && u.WalletType == coinName
                                select u);
                if (response.Count() != 1)
                {
                    i = false;
                    return i;
                }
                totalAmtDrTranx = response.ToList()[0].Amount;
                // total delivered amount _ current amount must less or equals total debit amount
                decimal deliveredAmt = (from p in _dbContext.WalletTransactionOrders
                                        join u in _dbContext.WalletTransactionQueues on p.DTrnNo equals u.TrnNo
                                        where u.TrnRefNo == arryTrnID[t].DrTrnRefNo && u.TrnType == Core.Enums.enWalletTranxOrderType.Debit
                                        && u.WalletType == coinName && p.Status != enTransactionStatus.SystemFail
                                        select p).Sum(e => e.Amount);
                if (!(totalAmtDrTranx - deliveredAmt - arryTrnID[t].Amount >= 0))
                {
                    i = false;
                    return i;
                }
                arryTrnID[t].dWalletId = response.ToList()[0].WalletID;
                arryTrnID[t].DrTQTrnNo = response.ToList()[0].TrnNo;

                i = true;
            }
            return i;
        }

        public List<AddressMasterResponse> ListAddressMasterResponse(string AccWalletID)
        {
            List<AddressMasterResponse> items = (from u in _dbContext.AddressMasters
                                                 join c in _dbContext.WalletMasters
                                                 on u.WalletId equals c.Id
                                                 where c.AccWalletID == AccWalletID && u.Status == 1
                                                 select new AddressMasterResponse
                                                 {
                                                     AddressLabel = u.AddressLable,
                                                     Address = u.Address,
                                                     IsDefaultAddress = u.IsDefaultAddress,
                                                 }).AsEnumerable().ToList();
            return items;
        }

        //vsolanki 16-10-2018
        public DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            List<HistoryObject> items = (from u in _dbContext.DepositHistory
                                         where u.UserId == Userid && u.CreatedDate >= FromDate && u.CreatedDate <= ToDate && (Status == null || (u.Status == Status && Status != null)) && (Coin == null || (u.SMSCode == Coin && Coin != null)) && (Amount == null || (u.Amount == Amount && Amount != null))
                                         select new HistoryObject
                                         {
                                             CoinName = u.SMSCode,
                                             Status = u.Status,
                                             Information = u.StatusMsg,
                                             Amount = u.Amount,
                                             Date = u.CreatedDate,
                                             Address = u.Address,
                                             StatusStr = (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 2) ? "OperatorFail" : (u.Status == 3) ? "SystemFail" : (u.Status == 4) ? "Hold" : (u.Status == 5) ? "Refunded" : "Pending"
                                         }).AsEnumerable().ToList();
            if (items.Count() == 0)
            {
                return new DepositHistoryResponse()
                {
                    ReturnCode = enResponseCode.Fail,
                    ReturnMsg = EnResponseMessage.NotFound,
                    ErrorCode = enErrorCode.NotFound
                };
            }

            return new DepositHistoryResponse()
            {
                ReturnCode = enResponseCode.Success,
                ReturnMsg = EnResponseMessage.FindRecored,
                Histories = items
            };
        }

        //vsolanki 16-10-2018
        public DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid)
        {
            List<HistoryObject> items = (from u in _dbContext.TransactionQueue
                                         where u.MemberID == Userid && u.TrnDate >= FromDate && u.TrnDate <= ToDate && (Status == null || (u.Status == Status && Status != null)) && (Coin == null || (u.SMSCode == Coin && Coin != null)) && (Amount == null || (u.Amount == Amount && Amount != null))
                                         select new HistoryObject
                                         {
                                             CoinName = u.SMSCode,
                                             Status = u.Status,
                                             Information = u.StatusMsg,
                                             Amount = u.Amount,
                                             Date = u.CreatedDate,
                                             Address = u.TransactionAccount,
                                             StatusStr = (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 2) ? "OperatorFail" : (u.Status == 3) ? "SystemFail" : (u.Status == 4) ? "Hold" : (u.Status == 5) ? "Refunded" : "Pending"
                                         }).AsEnumerable().ToList();

            if (items.Count() == 0)
            {
                return new DepositHistoryResponse()
                {
                    ReturnCode = enResponseCode.Fail,
                    ReturnMsg = EnResponseMessage.NotFound,
                    ErrorCode = enErrorCode.NotFound
                };
            }

            return new DepositHistoryResponse()
            {
                ReturnCode = enResponseCode.Success,
                ReturnMsg = EnResponseMessage.FindRecored,
                Histories = items
            };
        }
        public bool WalletCreditwithTQ(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2, WalletTransactionQueue wtq, CreditWalletDrArryTrnID[] arryTrnID)
        {
            try
            { // returns the address for ETH which are previously generated but not assinged to any wallet ntrivedi 26-09-2018

                _dbContext.Database.BeginTransaction();
                //_dbContext.Set<WalletTransactionQueue>().Add(wtq);
                //wl1.TrnNo = wtq.TrnNo;
                var arrayObj = (from p in _dbContext.WalletTransactionOrders
                                join q in arryTrnID on p.OrderID equals q.OrderID
                                select p).ToList();
                arrayObj.ForEach(e => e.Status = enTransactionStatus.Success);
                arrayObj.ForEach(e => e.StatusMsg = "Success");

                // update debit transaction(current tranx against which tranx) status if it is fully settled
                var arrayObjTQ = (from p in _dbContext.WalletTransactionQueues
                                  join q in arryTrnID on p.TrnNo equals q.DrTQTrnNo
                                  select new { p, q }).ToList();
                arrayObjTQ.ForEach(e => e.p.SettedAmt = e.p.SettedAmt + e.q.Amount);
                arrayObjTQ.ForEach(e => e.p.UpdatedDate = UTC_To_IST());
                arrayObjTQ.Where(d => d.p.SettedAmt >= d.p.Amount).ToList().ForEach(e => e.p.Status = enTransactionStatus.Success);


                _dbContext.Set<WalletLedger>().Add(wl1);
                _dbContext.Set<TransactionAccount>().Add(ta1);
                _dbContext.Entry(wm2).State = EntityState.Modified;
                _dbContext.Entry(wtq).State = EntityState.Modified;
                _dbContext.SaveChanges();
                _dbContext.Database.CommitTransaction();
                return true;
            }

            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }
        public DateTime UTC_To_IST()
        {
            try
            {
                DateTime myUTC = DateTime.UtcNow;
                // 'Dim utcdate As DateTime = DateTime.ParseExact(DateTime.UtcNow, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                // Dim utcdate As DateTime = DateTime.ParseExact(myUTC, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                // 'Dim utcdate As DateTime = DateTime.ParseExact("11/09/2016 6:31:00 PM", "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                DateTime istdate = TimeZoneInfo.ConvertTimeFromUtc(myUTC, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                // MsgBox(myUTC & " - " & utcdate & " - " & istdate)
                return istdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Rushabh 16-10-2018
        public List<WalletLimitConfigurationRes> GetWalletLimitResponse(string AccWaletID)
        {
            List<WalletLimitConfigurationRes> items = (from u in _dbContext.WalletLimitConfiguration
                                                       join c in _dbContext.WalletMasters
                                                       on u.WalletId equals c.Id
                                                       where c.AccWalletID == AccWaletID && u.Status == 1
                                                       select new WalletLimitConfigurationRes
                                                       {
                                                           TrnType = u.TrnType,
                                                           LimitPerDay = u.LimitPerDay,
                                                           LimitPerHour = u.LimitPerHour,
                                                           LimitPerTransaction = u.LimitPerTransaction,
                                                           AccWalletID = c.AccWalletID,
                                                           EndTime = u.EndTime,
                                                           StartTime = u.StartTime
                                                       }).AsEnumerable().ToList();
            return items;
        }

        public List<AddressMasterResponse> GetAddressMasterResponse(string AccWalletID)
        {
            List<AddressMasterResponse> items = (from u in _dbContext.AddressMasters
                                                 join c in _dbContext.WalletMasters
                                                 on u.WalletId equals c.Id
                                                 where c.AccWalletID == AccWalletID && u.IsDefaultAddress == 1 && u.Status == 1

                                                 select new AddressMasterResponse
                                                 {
                                                     AddressLabel = u.AddressLable,
                                                     Address = u.Address
                                                     //IsDefaultAddress = u.IsDefaultAddress,
                                                 }).AsEnumerable().ToList();
            if (items.Count() == 0)
            {
                List<AddressMasterResponse> items1 = (from u in _dbContext.AddressMasters
                                                      join c in _dbContext.WalletMasters
                                                      on u.WalletId equals c.Id
                                                      where c.AccWalletID == AccWalletID && u.Status == 1
                                                      orderby u.CreatedDate descending

                                                      select new AddressMasterResponse
                                                      {
                                                          AddressLabel = u.AddressLable,
                                                          Address = u.Address,
                                                          //IsDefaultAddress = u.IsDefaultAddress,
                                                      }).AsEnumerable().Take(1).ToList();
                return items1;
            }
            else
            {
                return items;
            }

        }

        //vsolanki 24-10-2018
        public List<BalanceResponse> GetAvailableBalance(long userid, long walletId)
        {
            List<BalanceResponse> items = (from w in _dbContext.WalletMasters
                                           join wt in _dbContext.WalletTypeMasters
                                                   on w.WalletTypeID equals wt.Id
                                           where w.Id == walletId && w.UserID == userid && w.Status == 1
                                           select new BalanceResponse
                                           {
                                               Balance = w.Balance,
                                               WalletId = w.Id,
                                               WalletType = wt.WalletTypeName
                                           }).AsEnumerable().ToList();
            return items;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetAllAvailableBalance(long userid)
        {
            List<BalanceResponse> items = (from w in _dbContext.WalletMasters
                                           join wt in _dbContext.WalletTypeMasters
                                                   on w.WalletTypeID equals wt.Id
                                           where w.UserID == userid && w.Status == 1
                                           select new BalanceResponse
                                           {
                                               Balance = w.Balance,
                                               WalletId = w.Id,
                                               WalletType = wt.WalletTypeName,
                                           }).AsEnumerable().ToList();
            return items;
        }

        public decimal GetTotalAvailbleBal(long userid)
        {
            var total = (from w in _dbContext.WalletMasters
                         where w.UserID == userid && w.Status == 1
                         select w.Balance
           ).Sum();
            return total;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetUnSettledBalance(long userid, long walletid)
        {

            var result = (from w in _dbContext.WalletTransactionQueues
                          where w.WalletID == walletid && w.MemberID == userid && w.Status == enTransactionStatus.Hold || w.Status == enTransactionStatus.Pending
                          group w by new { w.WalletType } into g
                          select new BalanceResponse
                          {
                              Balance = g.Sum(order => order.Amount),
                              WalletType = g.Key.WalletType,
                              WalletId = walletid
                          }).AsEnumerable().ToList();

            return result;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetAllUnSettledBalance(long userid)
        {
            var result = (from w in _dbContext.WalletTransactionQueues
                          where w.MemberID == userid && w.Status == enTransactionStatus.Hold || w.Status == enTransactionStatus.Pending
                          group w by new { w.WalletType, w.WalletID } into g
                          select new BalanceResponse
                          {
                              Balance = g.Sum(order => order.Amount),
                              WalletType = g.Key.WalletType,
                              WalletId = g.Key.WalletID
                          }).AsEnumerable().ToList();
            return result;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetUnClearedBalance(long userid, long walletid)
        {

            var result = (from w in _dbContext.DepositHistory
                          join wt in _dbContext.AddressMasters
                          on w.Address equals wt.Address
                          where wt.WalletId == walletid && w.UserId == userid && w.Status == 0
                          select new BalanceResponse
                          {
                              Balance = w.Amount,
                              WalletType = w.SMSCode,
                              WalletId = walletid
                          }).AsEnumerable().ToList();

            return result;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetUnAllClearedBalance(long userid)
        {
            var result = (from w in _dbContext.DepositHistory
                          join wt in _dbContext.AddressMasters
                          on w.Address equals wt.Address
                          where w.UserId == userid && w.Status == 0
                          select new BalanceResponse
                          {
                              Balance = w.Amount,
                              WalletType = w.SMSCode,
                              WalletId = wt.WalletId
                          }).AsEnumerable().ToList();
            return result;
        }
        //vsolanki 24-10-2018
        public List<StackingBalanceRes> GetStackingBalance(long userid, long walletid)
        {

            var result = (from u in _dbContext.UserStacking
                          join w in _dbContext.WalletMasters
                          on u.WalletId equals w.Id
                          where u.WalletId == walletid && w.UserID == userid && u.Status == 0
                          select new StackingBalanceRes
                          {
                              StackingAmount = u.StackingAmount,
                              WalletType = u.WalletType,
                              WalletId = walletid
                          }).AsEnumerable().ToList();

            if (result.Count() == 0)
            {
                var result1 = (from u in _dbContext.StckingScheme
                               join w in _dbContext.WalletMasters
                              on u.WalletType equals w.WalletTypeID
                               join wt in _dbContext.WalletTypeMasters
                               on u.WalletType equals wt.Id
                               where w.Id == walletid && w.UserID == userid && u.Status == 0
                               select new StackingBalanceRes
                               {
                                   MaxLimitAmount = u.MaxLimitAmount,
                                   MinLimitAmount = u.MinLimitAmount,
                                   WalletType = wt.WalletTypeName,
                                   WalletId = walletid
                               }).AsEnumerable().ToList();
                return result1;
            }

            return result;
        }
        //vsolanki 24-10-2018
        public List<StackingBalanceRes> GetAllStackingBalance(long userid)
        {
            var result = (from u in _dbContext.UserStacking
                          join w in _dbContext.WalletMasters
                          on u.WalletId equals w.Id
                          where w.UserID == userid && u.Status == 0
                          select new StackingBalanceRes
                          {
                              StackingAmount = u.StackingAmount,
                              WalletType = u.WalletType,
                              WalletId = w.Id
                          }).AsEnumerable().ToList();

            if (result.Count() == 0)
            {
                var result1 = (from u in _dbContext.StckingScheme
                               join w in _dbContext.WalletMasters
                              on u.WalletType equals w.WalletTypeID
                               join wt in _dbContext.WalletTypeMasters
                               on u.WalletType equals wt.Id
                               where w.UserID == userid && u.Status == 0
                               select new StackingBalanceRes
                               {
                                   MaxLimitAmount = u.MaxLimitAmount,
                                   MinLimitAmount = u.MinLimitAmount,
                                   WalletType = wt.WalletTypeName,
                                   WalletId = w.Id
                               }).AsEnumerable().ToList();
                return result1;
            }
            return result;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetShadowBalance(long userid, long walletid)
        {

            var result = (from u in _dbContext.MemberShadowBalance
                          join w in _dbContext.WalletMasters
                          on u.WalletID equals w.Id
                          join wt in _dbContext.WalletTypeMasters
                                                   on u.WalletTypeId equals wt.Id
                          where u.WalletID == walletid && w.UserID == userid && u.Status == 0
                          select new BalanceResponse
                          {
                              Balance = u.ShadowAmount,
                              WalletType = wt.WalletTypeName,
                              WalletId = walletid
                          }).AsEnumerable().ToList();

            if (result.Count() == 0)
            {
                var result1 = (from u in _dbContext.MemberShadowLimit
                               join w in _dbContext.BizUserTypeMapping
                               on u.MemberTypeId equals w.UserType
                               join wt in _dbContext.WalletMasters
                               on walletid equals wt.Id
                               join wtm in _dbContext.WalletTypeMasters
                                                  on wt.WalletTypeID equals wtm.Id
                               where u.WalletType == wt.WalletTypeID && w.UserID == userid && u.Status == 0
                               select new BalanceResponse
                               {
                                   Balance = u.ShadowLimitAmount,
                                   WalletType = wtm.WalletTypeName,
                                   WalletId = walletid
                               }).AsEnumerable().ToList();
                return result1;
            }

            return result;
        }
        //vsolanki 24-10-2018
        public List<BalanceResponse> GetAllShadowBalance(long userid)
        {
            var result = (from u in _dbContext.MemberShadowBalance
                          join w in _dbContext.WalletMasters
                          on u.WalletID equals w.Id
                          join wt in _dbContext.WalletTypeMasters
                                                   on u.WalletTypeId equals wt.Id
                          where w.UserID == userid && u.Status == 0
                          select new BalanceResponse
                          {
                              Balance = u.ShadowAmount,
                              WalletType = wt.WalletTypeName,
                              WalletId = w.Id
                          }).AsEnumerable().ToList();

            if (result.Count() == 0)
            {
                var result1 = (from u in _dbContext.MemberShadowLimit
                               join w in _dbContext.BizUserTypeMapping
                               on u.MemberTypeId equals w.UserType
                               join wt in _dbContext.WalletMasters
                               on u.WalletType equals wt.WalletTypeID
                               join wtm in _dbContext.WalletTypeMasters
                               on wt.WalletTypeID equals wtm.Id
                               where u.WalletType == wt.WalletTypeID && w.UserID == userid && u.Status == 0
                               select new BalanceResponse
                               {
                                   Balance = u.ShadowLimitAmount,
                                   WalletType = wtm.WalletTypeName,
                                   WalletId = wt.Id
                               }).AsEnumerable().ToList();
                return result1;
            }

            return result;
        }
        //vsolanki 24-10-2018
        public Balance GetAllBalances(long userid, long walletid)
        {
            var Unsettled = (from w in _dbContext.WalletTransactionQueues
                             where w.WalletID == walletid && w.MemberID == userid && w.Status == enTransactionStatus.Hold || w.Status == enTransactionStatus.Pending
                             select w.Amount).Sum();

            var availble = (from w in _dbContext.WalletMasters
                            join wt in _dbContext.WalletTypeMasters
                                    on w.WalletTypeID equals wt.Id
                            where w.Id == walletid && w.UserID == userid && w.Status == 1
                            select w.Balance).Sum();

            var UnClearedBalance = (from w in _dbContext.DepositHistory
                                    join wt in _dbContext.AddressMasters
                                    on w.Address equals wt.Address
                                    where wt.WalletId == walletid && w.UserId == userid && w.Status == 0
                                    select w.Amount
                          ).Sum();

            var ShadowBalance = (from u in _dbContext.MemberShadowBalance
                                 join w in _dbContext.WalletMasters
                                 on u.WalletID equals w.Id
                                 join wt in _dbContext.WalletTypeMasters
                                                          on u.WalletTypeId equals wt.Id
                                 where u.WalletID == walletid && w.UserID == userid && u.Status == 0
                                 select u.ShadowAmount).Sum();

            var StackingBalance = (from u in _dbContext.UserStacking
                                   join w in _dbContext.WalletMasters
                                   on u.WalletId equals w.Id
                                   where u.WalletId == walletid && w.UserID == userid && u.Status == 0
                                   select u.StackingAmount).Sum(); ;

            return new Balance { UnSettledBalance = Unsettled, AvailableBalance = availble, UnClearedBalance = UnClearedBalance, ShadowBalance = ShadowBalance, StackingBalance = StackingBalance };

        }

        //vsolanki 25-10-2018
        public List<BalanceResponseLimit> GetAvailbleBalTypeWise(long userid)
        {
            var result = (from w in _dbContext.WalletMasters
                          join wt in _dbContext.WalletTypeMasters
                          on w.WalletTypeID equals wt.Id
                          where w.UserID == userid && w.Status == 1
                          group w by new { wt.WalletTypeName } into g
                          select new BalanceResponseLimit
                          {
                              Balance = g.Sum(order => order.Balance),
                              WalletType = g.Key.WalletTypeName,
                          }).AsEnumerable().ToList();

            return result;
        }

        public List<BeneficiaryMasterRes> GetAllWhitelistedBeneficiaries(long WalletTypeID)
        {
            List<BeneficiaryMasterRes> items = (from b in _dbContext.BeneficiaryMaster
                                                where b.WalletTypeID == WalletTypeID && b.IsWhiteListed == 1
                                                select new BeneficiaryMasterRes
                                                {
                                                    Name = b.Name,
                                                    BeneficiaryID = b.Id,
                                                    Address = b.Address,
                                                    Status = b.Status

                                                }).AsEnumerable().ToList();
            return items;
        }

        public List<BeneficiaryMasterRes> GetAllBeneficiaries(long WalletTypeID)
        {
            List<BeneficiaryMasterRes> items = (from b in _dbContext.BeneficiaryMaster
                                                where b.WalletTypeID == WalletTypeID
                                                select new BeneficiaryMasterRes
                                                {
                                                    Name = b.Name,
                                                    BeneficiaryID = b.Id,
                                                    Address = b.Address,
                                                    IsWhiteListed = b.IsWhiteListed,
                                                    Status = b.Status

                                                }).AsEnumerable().ToList();
            return items;
        }

        public bool BeneficiaryBulkEdit(BulkBeneUpdateReq[] arryTrnID)
        {
            try
            {
                _dbContext.Database.BeginTransaction();
                var arrayObj = (from p in _dbContext.BeneficiaryMaster
                                join q in arryTrnID on p.Id equals q.ID
                                select new { p, q }).ToList();
                if (arrayObj.Count != 0)
                {
                    arrayObj.ForEach(e => e.p.IsWhiteListed = e.q.WhitelistingBit);
                    arrayObj.ForEach(e => e.p.UpdatedDate = UTC_To_IST());
                    _dbContext.SaveChanges();
                    _dbContext.Database.CommitTransaction();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                _log.LogError(ex, "An unexpected exception occured,\nMethodName:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\nClassname=" + this.GetType().Name, LogLevel.Error);
                throw ex;
            }
        }


        public void GetSetLimitConfigurationMaster(int[] AllowTrnType, long userid, long WalletId)
        {
            var arrayObj = (from p in _dbContext.WalletLimitConfigurationMaster
                            join q in AllowTrnType on p.TrnType equals q
                            select p).ToList();

            var fadd = from array in arrayObj
                       select new WalletLimitConfiguration
                       {
                           CreatedBy = userid,
                           CreatedDate = UTC_To_IST(),
                           WalletId = WalletId,
                           TrnType = array.TrnType,
                           LimitPerDay = array.LimitPerDay,
                           LimitPerHour = array.LimitPerHour,
                           LimitPerTransaction = array.LimitPerTransaction,
                           Status = Convert.ToInt16(ServiceStatus.Active),
                           StartTime = array.StartTime,
                           EndTime = array.EndTime,
                           LifeTime = null,
                           UpdatedDate = UTC_To_IST()
                       };
            _dbContext.WalletLimitConfiguration.AddRange(fadd);
            _dbContext.SaveChanges();

        }

        public void CreateDefaulWallet()
        {

        }

        //vsolanki 26-10-2018
        public DateTime UTC_To_IST(DateTime dateTime)
        {
            try
            {
                // DateTime myUTC = DateTime.UtcNow;
                // 'Dim utcdate As DateTime = DateTime.ParseExact(DateTime.UtcNow, "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                // Dim utcdate As DateTime = DateTime.ParseExact(myUTC, "M/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
                // 'Dim utcdate As DateTime = DateTime.ParseExact("11/09/2016 6:31:00 PM", "M/dd/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                DateTime istdate = TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
                // MsgBox(myUTC & " - " & utcdate & " - " & istdate)
                return istdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public decimal GetTodayAmountOfTQ(long userId, long WalletId)
        {
            DateTime startDateTime = UTC_To_IST(DateTime.UtcNow); //Today at 12:00:00
            DateTime endDateTime = UTC_To_IST(DateTime.UtcNow.AddDays(-1).AddTicks(-1));
            //var d = startDateTime.Date;
            //var amt = (from tq in _dbContext.WalletTransactionQueues
            //          where tq.Status == enTransactionStatus.Success && tq.TrnDate >= startDateTime &&
            //          tq.TrnDate <= endDateTime && tq.WalletID== WalletId && tq.MemberID==userId
            //          group tq by new { tq.TrnDate } into g
            //          select
            //          g.Sum(order => order.Amount));


            var total = (from tq in _dbContext.WalletTransactionQueues
                         where tq.Status == enTransactionStatus.Success && tq.TrnDate <= startDateTime.Date &&
                     tq.TrnDate >= endDateTime.Date && tq.WalletID == WalletId && tq.MemberID == userId
                         select tq.Amount
         ).Sum();
            return total;

            //return Convert.ToDecimal(amt);
        }

        //vsoalnki 26-10-2018
        public List<WalletLedgerRes> GetWalletLedger(DateTime FromDate, DateTime ToDate, long WalletId,int page)
        {
            //int skip = Helpers.PageSize * (page - 1);
            List<WalletLedgerRes> wl = (from w in _dbContext.WalletLedgers
                                        where w.WalletId == WalletId && w.TrnDate >= FromDate && w.TrnDate <= ToDate
                                        orderby w.TrnDate ascending
                                        select new WalletLedgerRes
                                        {
                                            LedgerId = w.Id,
                                            PreBal = w.PreBal,
                                            PostBal = w.PreBal,
                                            Remarks = "Opening Balance",
                                            Amount = 0,
                                            CrAmount=0,
                                            DrAmount=0,
                                           TrnDate=w.TrnDate
                                        }).Take(1).Union((from w in _dbContext.WalletLedgers
                                                  where w.WalletId == WalletId && w.TrnDate >= FromDate                        && w.TrnDate <= ToDate
                                                  select new WalletLedgerRes
                                                  {
                                                      LedgerId = w.Id,
                                                      PreBal = w.PreBal,
                                                      PostBal = w.PostBal,
                                                      Remarks = w.Remarks,
                                                      Amount = w.CrAmt > 0 ? w.CrAmt : w.DrAmt,
                                                      CrAmount = w.CrAmt,
                                                      DrAmount = w.DrAmt,
                                                      TrnDate = w.TrnDate
                                                  })).ToList();

            if (page > 0)
            {
                int skip = Helpers.PageSize * (page - 1);
                wl = wl.Skip(skip).Take(Helpers.PageSize).ToList();
            }
            return wl;
        }

    }
}
