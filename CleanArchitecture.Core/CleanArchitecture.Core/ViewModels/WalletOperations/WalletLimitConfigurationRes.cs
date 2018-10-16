using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WalletLimitConfigurationRes : BizResponseClass 
    {
        public string AccWalletID { get; set; }

        public int TrnType { get; set; }

        public decimal LimitPerHour { get; set; }

        public decimal LimitPerDay { get; set; }

        public decimal LimitPerTransaction { get; set; }
    }
}
