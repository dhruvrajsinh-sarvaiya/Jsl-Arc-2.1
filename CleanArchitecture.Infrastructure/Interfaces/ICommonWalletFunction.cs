using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Infrastructure.Interfaces
{
    public interface ICommonWalletFunction
    {
        decimal GetLedgerLastPostBal(long walletId);
        enErrorCode CheckShadowLimit(long WalletID, decimal Amount);
    }
}
