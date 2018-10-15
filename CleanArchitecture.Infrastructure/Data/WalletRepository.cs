using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
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

        public bool WalletDeductionwithTQ(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2,WalletTransactionQueue wtq)
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
                                                    Walletname = u.Walletname,
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
                                                    Walletname = u.Walletname,
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
                                                    Walletname = u.Walletname,
                                                    CoinName = c.WalletTypeName,
                                                    PublicAddress = u.PublicAddress,
                                                    Balance = u.Balance,
                                                    IsDefaultWallet = u.IsDefaultWallet
                                                }).AsEnumerable().ToList();
            return items;
        }


        public int  CheckTrnRefNo(long TrnRefNo, byte TrnType)
        {
            int response = (from u in _dbContext.WalletTransactionQueues
                                             where u.TrnRefNo == TrnRefNo && u.TrnType == TrnType
                                             select u).Count();
            return response;
        }

        public int CheckTrnRefNoForCredit(long TrnRefNo, byte TrnType)
        {
            int response = (from u in _dbContext.WalletTransactionQueues
                            where u.TrnRefNo == TrnRefNo && u.TrnType == TrnType && u.Status == 4
                            select u).Count();
            return response;
        }

        public WalletTransactionQueue AddIntoWalletTransactionQueue(WalletTransactionQueue wtq)
        {
            WalletTransactionQueue w = new WalletTransactionQueue();

             _dbContext.WalletTransactionQueues.Add(wtq);
            _dbContext.SaveChanges();
            return wtq;
        }

        public void CheckarryTrnID(CreditWalletDrArryTrnID[] arryTrnID)
        {
            //for (int t=0;t<=arryTrnID.Length;t++)
            //{ 
            //(from u in _dbContext.WalletTransactionQueues
            // where u.TrnRefNo == arryTrnID[0]  && u.Status == 4
            // select u).Count();
            //}
        }
    }
}
