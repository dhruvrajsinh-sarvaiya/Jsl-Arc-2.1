using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
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

        public WalletTransactionQueue AddIntoWalletTransactionQueue(WalletTransactionQueue wtq,byte AddorUpdate)//1=add,2=update
        {
            //WalletTransactionQueue w = new WalletTransactionQueue();
            if(AddorUpdate==1)
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
       public  WalletTransactionOrder AddIntoWalletTransactionOrder(WalletTransactionOrder wo, byte AddorUpdate)//1=add,2=update)
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
                                         where u.userId == Userid && u.CreatedDate >= FromDate && u.CreatedDate <= ToDate && (Status==null ||(u.Status==Status && Status != null)) && (Coin == null || (u.SMSCode == Coin && Coin != null)) && (Amount == null || (u.Amount == Amount && Amount != null))                                       
                                         select new HistoryObject
                                         {
                                             CoinName = u.SMSCode,
                                             Status = u.Status,
                                             Information=u.StatusMsg,
                                             Amount=u.Amount,
                                             Date=u.CreatedDate,
                                             Address=u.Address,
                                             StatusStr = (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 2) ? "OperatorFail" : (u.Status == 3) ? "SystemFail" : (u.Status == 4) ? "Hold" : (u.Status == 5) ? "Refunded" : "Pending"
                                         }).AsEnumerable().ToList();
            if(items.Count()==0)
            {
                return new DepositHistoryResponse()
                {
                    ReturnCode= enResponseCode.Fail,ReturnMsg= EnResponseMessage.NotFound,
                    ErrorCode = enErrorCode.NotFound
                };
            }

            return new DepositHistoryResponse()
            {
                ReturnCode = enResponseCode.Success,
                ReturnMsg = EnResponseMessage.FindRecored,
                Histories=items
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
                                             Address=u.TransactionAccount,
                                             StatusStr= (u.Status == 0) ? "Initialize" : (u.Status == 1) ? "Success" : (u.Status == 2) ? "OperatorFail" : (u.Status == 3) ? "SystemFail" : (u.Status == 4) ? "Hold" : (u.Status == 5) ? "Refunded" : "Pending"
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
                                select new {p,q }).ToList();
                arrayObjTQ.ForEach(e => e.p.SettedAmt = e.p.SettedAmt + e.q.Amount );
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
                                                           AccWalletID = c.AccWalletID
                                                       }).AsEnumerable().ToList();
            return items;
        }
    }
}
