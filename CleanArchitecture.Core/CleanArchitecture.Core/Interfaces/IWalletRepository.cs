﻿using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Interfaces
{
    public interface IWalletRepository
    {
        //T GetById(long id); moved to icommonrepository
        //List<T> List();
        //T Add(T entity);
        //void Update(T entity);
        //void Delete(T entity);
        //T AddProduct(T entity);
        
        TradeBitGoDelayAddresses GetUnassignedETH();
        bool WalletOperation(WalletLedger wl1, WalletLedger wl2, TransactionAccount ta1, TransactionAccount ta2, WalletMaster wm2, WalletMaster wm1);
        bool WalletDeduction(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2);
        bool WalletDeductionwithTQ(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2, WalletTransactionQueue wtq);
        List<WalletMasterResponse> ListWalletMasterResponse(long UserId);

        List<AddressMasterResponse> ListAddressMasterResponse(string AccWaletID); //Rushabh 15-10-2018

        List<WalletMasterResponse> GetWalletMasterResponseByCoin(long UserId, string coin);

        List<WalletMasterResponse> GetWalletMasterResponseById(long UserId, string coin,string walletId);

        int CheckTrnRefNo(long TrnRefNo, byte TrnType);

        int CheckTrnRefNoForCredit(long TrnRefNo, byte TrnType);

        WalletTransactionQueue AddIntoWalletTransactionQueue(WalletTransactionQueue wtq, byte AddorUpdate);

        WalletTransactionOrder AddIntoWalletTransactionOrder(WalletTransactionOrder wo, byte AddorUpdate);

        bool CheckarryTrnID(CreditWalletDrArryTrnID[] arryTrnID);

        //vsolanki 16-10-2018
        DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);
        //decimal GetCrSumAmtWallet(long walletid);
        //decimal GetDrSumAmtWallet(long walletid);



    }
}
