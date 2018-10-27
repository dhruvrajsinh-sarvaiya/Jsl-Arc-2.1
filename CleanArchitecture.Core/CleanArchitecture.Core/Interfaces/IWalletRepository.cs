using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Entities.Wallet;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.ViewModels.WalletOperations;
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

        int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType);

        int CheckTrnRefNoForCredit(long TrnRefNo, enWalletTranxOrderType TrnType);

        WalletTransactionQueue AddIntoWalletTransactionQueue(WalletTransactionQueue wtq, byte AddorUpdate);

        WalletTransactionOrder AddIntoWalletTransactionOrder(WalletTransactionOrder wo, byte AddorUpdate);

        bool CheckarryTrnID(CreditWalletDrArryTrnID[] arryTrnID,string coinName);

        //vsolanki 16-10-2018 
        DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //vsolanki 16-10-2018 
        DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //decimal GetCrSumAmtWallet(long walletid);
        //decimal GetDrSumAmtWallet(long walletid);
        bool WalletCreditwithTQ(WalletLedger wl1, TransactionAccount ta1, WalletMaster wm2, WalletTransactionQueue wtq, CreditWalletDrArryTrnID[] arryTrnID);
        List<WalletLimitConfigurationRes> GetWalletLimitResponse(string AccWaletID);
        List<AddressMasterResponse> GetAddressMasterResponse(string AccWaletID); //Rushabh 23-10-2018

        //vsolanki 24-10-2018
        List<BalanceResponse> GetAvailableBalance(long userid, long walletId);
        List<BalanceResponse> GetAllAvailableBalance(long userid);
        List<BalanceResponse> GetUnSettledBalance(long userid, long walletId);
        List<BalanceResponse> GetAllUnSettledBalance(long userid);
        List<BalanceResponse> GetUnClearedBalance(long userid, long walletId);
        List<BalanceResponse> GetUnAllClearedBalance(long userid);
        List<StackingBalanceRes> GetStackingBalance(long userid, long walletId);
        List<StackingBalanceRes> GetAllStackingBalance(long userid);
        List<BalanceResponse> GetShadowBalance(long userid, long walletId);
        List<BalanceResponse> GetAllShadowBalance(long userid);
        Balance GetAllBalances(long userid, long walletid);
        decimal GetTotalAvailbleBal(long userid);

        List<BeneficiaryMasterRes> GetAllWhitelistedBeneficiaries(long WalletTypeID);

        List<BeneficiaryMasterRes> GetAllBeneficiaries(long UserID);
        //vsolanki 25-10-2018
        List<BalanceResponseLimit> GetAvailbleBalTypeWise(long userid);

        bool BeneficiaryBulkEdit(BulkBeneUpdateReq[] arryTrnID);

        void GetSetLimitConfigurationMaster(int[] AllowTrnType, long userid, long WalletId);

        //vsolanki 24-10-2018 
        decimal GetTodayAmountOfTQ(long userId, long WalletId);

        List<WalletLedgerRes> GetWalletLedger(DateTime FromDate, DateTime ToDate, long WalletId, int page);

        int CreateDefaulWallet(long UserId);

        int CreateWalletForAllUser_NewService(string WalletType);

        object GetTypeMappingObj(long userid);
        
    }
}
