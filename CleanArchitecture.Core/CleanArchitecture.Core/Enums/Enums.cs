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

    public enum ModeStatus
    {
        False =0,
        True=1
    }

    public enum ServiceStatus
    {
        Disable = 9,
        Active = 1,
        InActive = 0
    }
    public enum enTransactionStatus
    {
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
        SMSAPI = 2
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
}
