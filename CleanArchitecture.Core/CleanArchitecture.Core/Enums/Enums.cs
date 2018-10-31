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
        Initialize = 0,
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
        Commission = 12
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
        LoanAPI = 16,
        Trading = 17
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
        LiquidityProvider = 3,
        PaymentGateway = 4,
        MarketData = 5,
        TradeServiceLocal = 6

    }

    //public enum enProviderAppType //Provider Apptype-Types of Transaction
    //{
    //    DemoCard = 1,
    //    WebService = 2,
    //    SocketBase = 3,
    //    AutoFill = 4,
    //    ThirdPartyAPI = 5,
    //    CyberPlate = 6,
    //    GTalkAPI = 7,
    //    DMRJBSPL = 8,
    //    DirectTrn = 9,
    //    HermesMobileAPI = 10,
    //    HermesFlightAPI = 11,
    //    HermesBusAPI = 12,
    //    LoanAPI = 24,
    //    AEPSTopUpCall = 26,
    //}

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
        Success = 2253,
        ItemNotFoundForGenerateAddress = 2254,
        InvalidThirdpartyID = 2255,
        AddressGenerationFailed = 2256,
        //====================Transactional
        TransactionProcessSuccess = 4566,
        TransactionInsertSuccess = 4567,
        TransactionInsertFail = 4568,
        TransactionInsertInternalError = 4569,
        CreateTrn_NoPairSelected = 4570,
        CreateTrnInvalidQtyPrice = 4571,
        CreateTrnInvalidQtyNAmount = 4572,
        CreateTrn_NoCreditAccountFound = 4573,
        CreateTrn_NoDebitAccountFound = 4574,
        CreateTrnInvalidAmount = 4575,
        CreateTrnDuplicateTrn = 4576,
        CreateTrn_NoSelfAddressWithdrawAllow = 4577,
        InvalidPairName = 4500,
        NoDataFound = 4501,
        InvalidStatusType = 4502,
        InValidTrnType = 4503,
        InvalidFromDateFormate = 4504,
        InvalidToDateFormate = 4505,
        InvalidMarketType = 4506,
        InValidPageNo = 4507,
        InValid_ID = 4565,

        ProcessTrn_InsufficientBalance = 4578,
        ProcessTrn_AmountBetweenMinMax = 4579,
        ProcessTrn_PriceBetweenMinMax = 4580,
        ProcessTrn_InvalidBidPriceValue = 4581,
        ProcessTrn_PoolOrderCreateFail = 4582,
        ProcessTrn_Initialize = 4583,
        TransactionProcessInternalError = 4584,
        ProcessTrn_ServiceProductNotAvailable = 4585,
        ProcessTrn_WalletDebitFail = 4586,
        ProcessTrn_Hold = 4587,
        ProcessTrn_ThirdPartyDataNotFound = 4588,
        ProcessTrn_GettingResponseBlank = 4589,
        ProcessTrn_OprFail = 4590,
        TradeRecon_InvalidTransactionNo = 4591,
        TradeRecon_After7DaysTranDontTakeAction = 4593,
        TradeRecon_InvalidTransactionStatus = 4594,
        TradeRecon_CancelRequestAlreayInProcess = 4595,
        TradeRecon_TransactionAlreadyInProcess = 4596,
        TradeRecon_OrderIsFullyExecuted = 4597,
        TradeRecon_InvalidDeliveryAmount = 4598,
        TradeRecon_CencelRequestSuccess = 4600,
        TradeRecon_InvalidActionType = 4618,
        FavPair_InvalidPairId = 4619,
        FavPair_AlreadyAdded = 4620,
        FavPair_AddedSuccess = 4621,
        FavPair_RemoveSuccess = 4622,
        FavPair_NoPairFound = 4623,
        InValidDebitAccountID= 4624,
        InValidCreditAccountID = 4625,
        //=======================
        //=====================MyAccount
        Status500InternalServerError = 500,
        Status400BadRequest = 400,
        Status423Locked = 423,
        Status4013MobileInvalid = 4013,
        Status4020IpInvalid = 4020,
        Status4032LoginFailed = 4032,
        Status4033NotFoundRecored = 4033,
        Status4034UnableUpdateUser = 4034,
        Status4035UnableToAddIpAddress = 4035,
        Status4036VerifyPending = 4036,
        Status4037UserNotAvailable = 4037,
        Status4038ResetUserNotAvailable = 4038,
        Status4039ResetPasswordLinkExpired = 4039,
        Status4040ResetPasswordLinkempty = 4040,
        Status4041ResetPasswordConfirm = 4041,
        Status4042ResetConfirmPassMatch = 4042,
        Status4043ResetConfirmOldNotMatch = 4043,
        Status4044UserSelectedIpNotFound = 4044,
        Status4045InvalidUserSelectedIp = 4045,
        Status4046NotUpdateIpStatus = 4046,
        Status4047NotDeleteIp = 4047,
        Status4048Invalidappkey = 4048,
        Status4049appkey = 4049,
        Status4050InvalidUser = 4050,
        Status4051RefreshToken = 4051,
        Status4052UserToken = 4052,
        Status4053Granttype = 4053,
        Status4054FactorFail = 4054,
        Status4055DisableTroFactorError = 4055,
        Status4056TwoFactorVerification = 4056,
        Status4057DeviceIdNotAdd = 4057,
        Status4058DeviceAddress = 4058,
        Status4059NotDeleteDevice = 4059,
        Status4060VerifyMethod = 4060,
        Status4061Userpasswordnotupdated = 4061,
        Status4062UseralreadRegister = 4062,
        Status4063UserNotRegister = 4063,
        Status4064EmailLinkBlanck = 4064,

        Status4066UserRoleNotAvailable = 4066,
        Status4067InvalidOTPorexpired = 4067,
        Status4068InvalidGoogleToken = 4068,
        Status4069InvalidGoogleProviderKey = 4069,
        Status4070SocialUserInsertError = 4070,
        Status4071TwoFactorVerificationDisable = 4071,
        Status4074SignUPMobileValidation = 4074,
        Status4075SignUPOTP = 4075,
        Status4076SignUpReSendOTP = 4076,
        Status4077UserUnlockError = 4077,
        Status4078SignUpRole = 4078,
        Status4081IpAddressNotInsert = 4081,
        Status4083IpAddressExist = 4083,
        Status4084DeviceIdExist = 4084,
        Status4079TwoFAcodeInvalide = 4079,
        Status4085LoginWithOtpDatanotSend = 4085,
        Status4086LoginWithOtpLoginFailed = 4086,
        Status4087EmailFail = 4087,
        Status4088LoginWithOtpInvalidAttempt = 4088,
        Status4089LoginEmailOTPNotsend = 4089,
        Status4090OTPSendOnMobile = 4090,
        Status4091LoginMobileNumberInvalid = 4091,
        Status4096InvalidFaceBookToken = 4096,
        Status4097InvalidFaceBookProviderKey = 4097,

        Status4098BizUserEmailExist = 4098,
        Status4099BizUserNameExist = 4099,
        Status4100provideDetailNotAvailable = 4100,
        Status4101InputProvider = 4101,
        Status4102SignUpUserRegisterError = 4102,

        Status4103UserDataNotAvailable = 4103,
        Status4104TempUserNameExist = 4104,
        Status4105TempUserMobileExist = 4105,
        Status4106LoginFailMobileNotAvailable = 4106,

        ///// wallet ///////////
        DeductWalletNullWalletIDorCoinType = 424,
        DefaultWalletNotFound = 425,
        BatchNoGenerationFailed = 246,
        InvalidWalletId = 247,
        InvalidTrnType = 248,
        TrnTypeNotAllowed = 249,
        RecordNotFound = 250,
        InvalidCoinName = 4031,
        StandardLoginfailed = 4032,
        NotFound = 4033,
        InvalidWalletOrUserIDorCoinName = 4034,
        InvalidWallet = 4035,
        InvalidTradeRefNo = 4036,
        AlredyExist = 4037,
        InsufficantBal = 4038,
        UserIDWalletIDDidNotMatch = 4039,
        InvalidBeneficiaryID = 4227,
        InvalidLimit = 4233,
        InternalError=9,
        NotFoundLimit=4280,
        DuplicateRecord=4281,
        AddressNotFoundOrWhitelistingBitIsOff = 4237,
        BeneficiaryNotFound = 4238,
        AddressNotMatch = 4239,
        GlobalBitNotFound = 4240,
        WalletNotFound = 4241,
        //NotFoundLimit=4234,
        //DuplicateRecord=4235,       
        InvalidAddress = 4236,
        OrgIDNotFound = 2427,
        MemberTypeNotFound = 4242,
        SettedBalanceMismatch  = 4243,
        ShadowBalanceExceed = 4244
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
        BitcoinDeamon = 7,
        COINTTRADINGLocal = 8
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
        Cr_Bonus = 16,
        Dr_Debit = 17 //ntrivedi for admin wallet deduct when deposition done 
    }

    public enum enTransactionMarketType
    {
        LIMIT = 1,
        MARKET = 2,
        STOP_LOSS = 3,
        SPOT = 4
    }
    public enum enWalletTranxOrderType
    {
        Debit = 1,
        Credit = 2
    }

    public enum enWalletLimitType
    {
        TradingLimit = 1,
        WithdrawLimit = 2,
        DepositLimit = 3,
        APICallLimit = 4
    }
    public enum enSignalREventType
    {
        Alert = 1,
        Nofification = 2,
        GrpMsg = 3,
        Channel = 4,
        BroadCast = 5
    }
    
    public enum enSubscriptionType
    {
        OneToOne = 1,
        Broadcast = 2
    }
    public enum enMethodName
    {
        OpenOrder = 1,
        OrderHistory = 2,
        TradeHistory = 3,
        ActivityNotification = 4,
        //PairWise
        BuyerBook = 5,
        SellerBook = 6,
        TradeHistoryByPair = 7,
        MarketData = 8,
        Price = 9,
        BuyerSideWallet = 10,
        SellerSideWallet = 11,
        ChartData = 12,
        //Market wise
        PairData = 13,
        MarketTicker = 14,
        //Global 
        Chat = 15,
        News = 16,
        Announcement = 17,
        Time = 18
    }
    public enum PairWiseMethodName
    {
        BuyerBook = 5,
        SellerBook = 6,
        TradingHistory = 7,
        MarketData = 8,
        Price = 9,
        BuyerSideWallet = 10,
        SellerSideWallet = 11,
        ChartData = 12
    }
    public enum MarketWiseMethodName
    {
        PairData = 13,
        MarketTicker = 14
    }
    public enum GlobalMethodName
    {
        Chat = 15,
        News = 16,
        Announcement = 17,
        Time = 18
    }
    public enum enValidateWalletLimit
    {
        Success = 1,
        Fail = 2
    }
    public enum enSignalRParmType
    {
        PairName = 1,
        Base = 2,
        AccessToken = 3
    }
    public enum enReturnMethod
    {
        //user specific
        RecieveOpenOrder = 1,
        RecieveOrderHistory = 2,
        RecieveTradeHistory = 3,
        RecieveBuyerSideWalletBal = 4,
        RecieveSellerSideWalletBal = 5,
        RecieveNotification=14,

        //pair wise
        RecieveBuyerBook = 6,
        RecieveSellerBook = 7,
        RecieveTradingHistory = 8,
        RecieveMarketData = 9,
        RecieveChartData = 10,
        RecieveLastPrice = 11,

        //Base Market
        RecievePairData = 12,
        RecieveMarketTicker = 13,
        BroadcastMessage = 14
    }
    public enum enCheckWithdrawalBene
    {
        Success = 1,
        WalletNotFound = 2,
        GlobalBitNotFound = 3,
        BeneficiaryNotFound = 4,
        AddressNotFoundOrWhitelistingBitIsOff = 5,
        AddressNotMatch = 6
    }
    //vsolanki 2018-10-29
    public enum enUserType
    {
        Organization=0,
        User=1
    }

    public enum enWhiteListingBit
    {
        OFF = 0,
        ON = 1
    }

    /// <summary>
    ///  This enum type are create by pankaj for required to tract the user log for type so.
    /// </summary>
    public enum EnuserChangeLog
    {
        UserProfile = 1,
        TwofactoreChange = 2,
        ChangePassword = 3,
        SetPassword = 4

    }

    public enum enTradeReconActionType
    {
        Refund = 1,
        SuccessAndDebit = 2,
        Success = 3,
        FaildeMark = 5,
        PartialRefund = 7,
        Cancel = 8
    }

    public enum enChargeType
    {
        Fixed = 1,
        Pecentage = 2
    }
}
