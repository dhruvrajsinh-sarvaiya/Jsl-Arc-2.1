using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Enums
{
    public enum ExternalLoginStatus
    {
        Ok = 0,
        Error = 1,
        Invalid = 2,
        TwoFactor = 3,
        Lockout = 4,
        CreateAccount = 5
    }  

    public enum ServiceStatus
    {
        Disable = 9,
        Active = 1,
        InActive = 0
    }
    public enum enTransactionStatus
    {
        Initialize=0,
        Success = 1,
        OperatorFail = 2,
        SystemFail = 3,
        Hold = 4,
        Refunded = 5,
        Pending = 6
    }
    public enum enTrnType
    {
        Transaction = 1,
        Buy_Trade = 4,
        Sell_Trade = 5,
        Withdraw = 6,
        Shoping_Cart = 7,
        Deposit = 8,
        Generate_Address = 9,
        Topup = 10,
        Charge = 11,
        Commission=12        
    }
    public enum enOrderStatus
    {
        Rejected = 9,
        Success = 1,
        Pending = 0
    }

    public enum enOrderType
    {
        BuyOrder = 1,
        SellOrder = 2,
        CancelOrder = 3,
        DepositOrder = 4,
        WithdrawalOrder = 5,
        CashOnBank = 6,
        Cheque = 7,
        Transfer = 8
    }

    public enum enServiceType
    {
        Recharge = 1,        
        DTH = 2,
        BillPayment = 3,
        FlightBooking = 4,
        RailwayBooking = 5,
        BusBooking = 6,
        HotelBooking = 7,
        DataCard = 8,
        DMRSERVICE = 13,
        CAB = 14,
        WalletService = 15,
        LoanAPI = 16
        //Buy_Trade = 4,
        //Sell_Trade = 5,
        //Withdraw = 6,
        //Shoping_Cart = 7,
        //Deposit = 8,
        //Generate_Address = 9
    }

    public enum MessageStatusType
    {
        Pending = 6,
        Success = 1,
        Initialize = 0,
        Fail = 9
    }
    public enum enWebAPIRouteType //ThirdParty Apptype-Types of API
    {
        TransactionAPI = 1,
        CommunicationAPI = 2,
        LiquidityProvider=3,
        PaymentGateway=4,
        MarketData=5

    }

    public enum enProviderAppType //Provider Apptype-Types of Transaction
    {
        DemoCard = 1,
        WebService = 2,
        SocketBase = 3,
        AutoFill = 4,
        ThirdPartyAPI = 5,
        CyberPlate = 6,
        GTalkAPI = 7,
        DMRJBSPL = 8,
        DirectTrn = 9,
        HermesMobileAPI = 10,
        HermesFlightAPI = 11,
        HermesBusAPI = 12,
        LoanAPI = 24,
        AEPSTopUpCall = 26,
    }

    public enum enMessageService
    {
        Init = 0,
        Success = 1,
        Fail = 2,
        Pending = 6        
    }

    public enum enErrorCode
    {
        InvalidAmount = 2251,
        InsufficientBalance = 2252,
        InvalidWallet = 2253,
        Success = 2253,
        ItemNotFoundForGenerateAddress = 2254,
        InvalidThirdpartyID = 2255,
        AddressGenerationFailed = 2256,
        //====================Transactional
        TransactionProcessSuccess = 1,
        TransactionInsertSuccess = 1,
        TransactionInsertFail =1,
        TransactionInsertInternalError =1,
        CreateTrn_NoPairSelected = 1,
        CreateTrnInvalidQtyPrice = 1,
        CreateTrnInvalidQtyNAmount = 1,
        CreateTrn_NoCreditAccountFound = 1,
        CreateTrn_NoDebitAccountFound = 1,
        CreateTrnInvalidAmount = 1,
        CreateTrnDuplicateTrn=1,
        CreateTrn_NoSelfAddressWithdrawAllow=1,
        //=======================
        //=====================MyAccount
        Status500InternalServerError = 500,
        Status400BadRequest = 400,
        Status423Locked = 423,
        // wallet
        DeductWalletNullWalletIDorCoinType = 424,
        DefaultWalletNotFound = 425,
        BatchNoGenerationFailed = 246,
        InvalidWalletId = 247,
        InvalidTrnType = 248,
        TrnTypeNotAllowed = 249,
            InvalidCoinName=4031
    }
    //Rushabh 05-10-2018 as per instruction by nupoora mam change Enum 0 for Success and 1 for Fail
    public enum enResponseCodeService
    {
        Success = 0, 
        Fail = 1,
        InternalError = 9
    }

    public enum enResponseCode
    {
        Success = 0,
        Fail = 1,
        InternalError = 9
    }

    public enum enRegisterType
    {
        Mobile = 1,
        Email = 2,
        Standerd = 3,
    }

    public enum enCommunicationServiceType
    {
        SMS = 1,
        Email = 2,
        PushNotification = 3,
    }

    public enum enumHistoryType
    {
        Login = 1,
    }

    public enum enAppType //Use in SerProDetail table
    {
        WebSocket = 1,
        JsonRPC = 2,
        TCPSocket = 3,
        RestAPI = 4,
        HttpApi = 5,
        SocketApi = 6,
        BitcoinDeamon = 7
    }

    public enum enServiceMasterType
    {
        Coin = 1,
        Token = 2
    }

    public enum enWalletTrnType
    {
        Cr_Topup = 1,
        cr_Deposit = 2,
        Cr_Buy_Trade = 3,
        Cr_Refund = 4,        
        Cr_Commission = 5,
        Cr_Partial_Cancel = 6,
        Cr_Trans_IN = 7,
        Dr_Sell_Trade = 8,
        Dr_Withdrawal = 9,
        Dr_Ecommerce = 10,
        Dr_Charges = 11,
        Dr_Trans_OUT = 12,
        Dr_Freeze = 13,
        Dr_Stacking = 14,
        Dr_ESCROW = 15,
        Cr_Bonus = 16
    }
}
