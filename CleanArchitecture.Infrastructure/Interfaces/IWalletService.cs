using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.ViewModels.Wallet;
using CleanArchitecture.Core.ViewModels.WalletOperations;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    
    public interface IWalletService
    {
        decimal GetUserBalance(long walletId);

        bool WalletBalanceCheck(decimal amount, long walletId);

        bool IsValidWallet(long walletId);

        CreateWalletAddressRes GenerateAddress(string walletid,string coin);

        //vsolanki 8-10-2018
        IEnumerable<WalletTypeMaster> GetWalletTypeMaster();

        //vsolanki 10-10-2018
        CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId);

        //ntrivedi 11-10-2018
        BizResponseClass DebitBalance(long userID, long WalletID, decimal amount, int walletTypeID, enWalletTrnType wtrnType, enTrnType trnType, enServiceType serviceType, long trnNo, string smsCode);

        ListWalletResponse ListWallet(long userid);

        ListWalletResponse GetWalletByCoin(long userid, string coin);

        ListWalletResponse GetWalletById(long userid, string coin,string walletId);

        WalletDrCrResponse GetWalletDeductionNew(string coinName, string timestamp, enWalletTranxOrderType orderType, decimal amount, long userID, string accWalletID, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType);

        ListWalletAddressResponse ListAddress(string AccWalletID);

        //vsolanki 16-10-2018
        DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //vsolanki 16-10-2018
        DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //ntrivedi 16-10-2018
        WalletDrCrResponse GetWalletCreditNew(string coinName, string timestamp, enWalletTrnType trnType, decimal TotalAmount, long userID, string crAccWalletID, CreditWalletDrArryTrnID[] arryTrnID, long TrnRefNo, short isFullSettled, enWalletTranxOrderType orderType, enServiceType serviceType);

        //Rushabh 16-10-2018
        LimitResponse SetWalletLimitConfig(string accWalletID, WalletLimitConfigurationReq request, long userID);

        //Rushabh 16-10-2018
        LimitResponse GetWalletLimitConfig(string accWalletID);

        //vsolanki 18-10-2018
        WithdrawalRes Withdrawl(string coin, string accWalletID, WithdrawalReq Request, long userid);

        ListWalletAddressResponse GetAddress(string AccWalletID);

        //vsolanki 24-10-2018
        ListBalanceResponse GetAvailableBalance(long userid, long walletId);
        ListBalanceResponse GetAllAvailableBalance(long userid);

        ListBalanceResponse GetUnSettledBalance(long userid, long walletId);
        AllBalanceResponse GetAllBalances(long userid, long walletId);
    }
}