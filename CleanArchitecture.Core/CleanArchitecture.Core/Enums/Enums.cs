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
    public enum TransactionStatus
    {
        Success = 1,
        OperatorFail = 2,
        SystemFail = 3,
        Hold = 4,
        Refunded = 5,
        Pending = 6
    }
    public enum EnTrnType
    {
        Transaction = 1,
        Buy_Trade = 4,
        Sell_Trade = 5,
        Withdraw = 6,
        Shoping_Cart = 7,
        Deposit = 8,
        Generate_Address = 9
    }
    public enum EnOrderStatus
    {
        Rejected = 9,
        Success = 1,
        Pending = 0
    }

    public enum EnOrderType
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

    public enum EnServiceType
    {
        Recharge = 1,
        BillPayment = 2,
        DTH = 3,
        Buy_Trade = 4,
        Sell_Trade = 5,
        Withdraw = 6,
        Shoping_Cart = 7,
        Deposit = 8,
        Generate_Address = 9
    }

    public enum MessageStatusType
    {
        Pending = 6,
        Success = 1,
        Initialize = 0,
        Fail = 9
    }
}
