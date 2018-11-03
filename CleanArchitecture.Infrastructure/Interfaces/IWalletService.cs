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

namespace CleanArchitecture.Infrastructure.Interfaces
{

    public interface IWalletService
    {
        decimal GetUserBalance(long walletId);

        bool WalletBalanceCheck(decimal amount, string walletId);

        bool IsValidWallet(long walletId);

        CreateWalletAddressRes GenerateAddress(string walletid,string coin, string Token, int GenaratePendingbit = 0);

        //vsolanki 8-10-2018
        IEnumerable<WalletTypeMaster> GetWalletTypeMaster();

        //vsolanki 10-10-2018
        CreateWalletResponse InsertIntoWalletMaster(string Walletname, string CoinName, byte IsDefaultWallet, int[] AllowTrnType, long userId, int isBaseService = 0);

        //ntrivedi 11-10-2018
        BizResponseClass DebitBalance(long userID, long WalletID, decimal amount, int walletTypeID, enWalletTrnType wtrnType, enTrnType trnType, enServiceType serviceType, long trnNo, string smsCode);

        ListWalletResponse ListWallet(long userid);

        ListWalletResponse GetWalletByCoin(long userid, string coin);

        ListWalletResponse GetWalletById(long userid, string coin, string walletId);

        WalletDrCrResponse GetWalletDeductionNew(string coinName, string timestamp, enWalletTranxOrderType orderType, decimal amount, long userID, string accWalletID, long TrnRefNo, enServiceType serviceType, enWalletTrnType trnType, enTrnType routeTrnType, string Token = "");

        ListWalletAddressResponse ListAddress(string AccWalletID);

        //vsolanki 16-10-2018
        DepositHistoryResponse DepositHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //vsolanki 16-10-2018
        DepositHistoryResponse WithdrawalHistoy(DateTime FromDate, DateTime ToDate, string Coin, decimal? Amount, byte? Status, long Userid);

        //ntrivedi 16-10-2018
        WalletDrCrResponse GetWalletCreditNew(string coinName, string timestamp, enWalletTrnType trnType, decimal TotalAmount, long userID, string crAccWalletID, CreditWalletDrArryTrnID[] arryTrnID, long TrnRefNo, short isFullSettled, enWalletTranxOrderType orderType, enServiceType serviceType, string Token = "");

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

        BeneficiaryResponse AddBeneficiary(string CoinName, short WhitelistingBit, string Name, string BeneficiaryAddress, long UserId);
        BeneficiaryResponse ListWhitelistedBeneficiary(string accWalletID, long id);
        BeneficiaryResponse ListBeneficiary(long id);
        UserPreferencesRes SetPreferences(long Userid, int GlobalBit);
        UserPreferencesRes GetPreferences(long Userid);
        BeneficiaryResponse UpdateBulkBeneficiary(BulkBeneUpdateReq request, long id);
        BeneficiaryResponse UpdateBeneficiaryDetails(BeneficiaryUpdateReq request, string AccWalletID, long id);
        //vsolanki 25-10-2018
        ListAllBalanceTypeWiseRes GetAllBalancesTypeWise(long userId, string WalletType);

        ListWalletLedgerRes GetWalletLedger(DateTime FromDate, DateTime ToDate, string WalletId, int page);

        BizResponseClass CreateDefaulWallet(long UserID);
        BizResponseClass CreateWalletForAllUser_NewService(string WalletType);

        //vsolanki 2018-10-29
        BizResponseClass AddBizUserTypeMapping(AddBizUserTypeMappingReq req);

        long GetWalletID(string AccWalletID);
        string GetAccWalletID(long WalletID);
        enErrorCode CheckWithdrawalBene(long WalletID, string Name, string DestinationAddress, enWhiteListingBit WhitelistingBit);
        //enCheckWithdrawalBene CheckWithdrawalBene(long WalletID, string Name, string DestinationAddress, short WhitelistingBit);

        WalletTransactionQueue InsertIntoWalletTransactionQueue(Guid Guid, enWalletTranxOrderType TrnType, decimal Amount, long TrnRefNo, DateTime TrnDate, DateTime? UpdatedDate,
           long WalletID, string WalletType, long MemberID, string TimeStamp, enTransactionStatus Status, string StatusMsg, enWalletTrnType enWalletTrnType);

        int CheckTrnRefNo(long TrnRefNo, enWalletTranxOrderType TrnType, enWalletTrnType wType);

        //vsolanki 2018-10-29
        ListIncomingTrnRes GetIncomingTransaction(long Userid, string Coin);

        // ntrivedi 29102018
        long GetWalletByAddress(string address);

        // ntrivedi 29102018

        WalletLedger GetWalletLedgerObj(long WalletID, long toWalletID, decimal drAmount, decimal crAmount, enWalletTrnType trnType, enServiceType serviceType, long trnNo, string remarks, decimal currentBalance, byte status);
        // ntrivedi 29102018

        TransactionAccount GetTransactionAccount(long WalletID, short isSettled, long batchNo, decimal drAmount, decimal crAmount, long trnNo, string remarks, byte status);
        // ntrivedi 29102018

        WalletTransactionOrder InsertIntoWalletTransactionOrder(DateTime? UpdatedDate, DateTime TrnDate, long OWalletID, long DWalletID, decimal Amount, string WalletType, long OTrnNo, long DTrnNo, enTransactionStatus Status, string StatusMsg);

        //vsolanki 2018-10-29
        bool CheckUserBalance(long WalletId);

        //Uday 30-10-2018
        ServiceLimitChargeValue GetServiceLimitChargeValue(enTrnType TrnType, string CoinName);

        //vsoalnki 2018-10-31
        CreateWalletAddressRes CreateETHAddress(string Coin, int AddressCount, long UserId);

        ListOutgoingTrnRes GetOutGoingTransaction(long Userid, string Coin);

        //vsolanki 2018-11-03
        ListTokenConvertHistoryRes ConvertFundHistory(long Userid, DateTime FromDate, DateTime ToDate, string Coin);

        //vsolanki 2018-11-03
        decimal ConvertFund(decimal SourcePrice);
        BizResponseClass AddIntoConvertFund(ConvertTockenReq Request, long userid);
    }
}