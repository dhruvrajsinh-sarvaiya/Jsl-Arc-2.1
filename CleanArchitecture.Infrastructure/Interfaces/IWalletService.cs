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

        bool WalletBalanceCheck(decimal amount, string walletId);

        bool IsValidWallet(long walletId);

        CreateWalletAddressRes GenerateAddress(string walletid,string coin);

        //vsolanki 8-10-2018
        IEnumerable<WalletTypeMaster> GetWalletTypeMaster();

        //vsolanki 10-10-2018
        CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId, int isBaseService = 0);

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
        ListBalanceResponse GetAvailableBalance(long userid, string walletId);
        TotalBalanceRes GetAllAvailableBalance(long userid);
        //vsolanki 24-10-2018
        ListBalanceResponse GetUnSettledBalance(long userid, string walletId);
        ListBalanceResponse GetAllUnSettledBalance(long userid);
        //vsolanki 24-10-2018
        ListBalanceResponse GetUnClearedBalance(long userid, string walletId);
        ListBalanceResponse GetAllUnClearedBalance(long userid);
        //vsolanki 24-10-2018
        ListStackingBalanceRes GetStackingBalance(long userid, string walletId);
        ListStackingBalanceRes GetAllStackingBalance(long userid);
        //vsolanki 24-10-2018
        ListBalanceResponse GetShadowBalance(long userid, string walletId);
        ListBalanceResponse GetAllShadowBalance(long userid);
        //vsolanki 24-10-2018
        AllBalanceResponse GetAllBalances(long userid, string walletId);
        // vsolanki 25-10-2018
        BalanceResponseWithLimit GetAvailbleBalTypeWise(long userid);

        BeneficiaryResponse AddBeneficiary(string AccWalletID, string BeneficiaryAddress, long UserId);
        BeneficiaryResponse ListWhitelistedBeneficiary(string accWalletID, long id);
        BeneficiaryResponse ListBeneficiary(long id);
        UserPreferencesRes SetPreferences(long Userid, int GlobalBit);
        UserPreferencesRes GetPreferences(long Userid);
        BeneficiaryResponse UpdateBulkBeneficiary(BulkBeneUpdateReq[] request, long id);
        BeneficiaryResponse UpdateBeneficiaryDetails(BeneficiaryUpdateReq request,string AccWalletID, long id);
        //vsolanki 25-10-2018
        List<AllBalanceTypeWiseRes> GetAllBalancesTypeWise(long userId, string WalletType);

        ListWalletLedgerRes GetWalletLedger(DateTime FromDate, DateTime ToDate, string WalletId, int page);

        BizResponseClass CreateDefaulWallet(long UserID);
    }
}