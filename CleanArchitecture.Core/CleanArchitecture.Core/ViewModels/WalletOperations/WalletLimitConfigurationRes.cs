using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WalletLimitConfigurationRes  
    {
        public string AccWalletID { get; set; }

        public int TrnType { get; set; }

        public decimal LimitPerHour { get; set; }

        public decimal LimitPerDay { get; set; }

        public decimal LimitPerTransaction { get; set; }

        public double? StartTime { get; set; }

        public double? EndTime { get; set; }

        public decimal? LifeTime { get; set; }
    }


    //public class WalletLimitConfigurationRes2
    //{
    //    public string AccWalletID { get; set; }

    //    public int TrnType { get; set; }

    //    public decimal LimitPerHour { get; set; }

    //    public decimal LimitPerDay { get; set; }

    //    public decimal LimitPerTransaction { get; set; }

    //    public long StartTime { get; set; }

    //    public long EndTime { get; set; }

    //    public decimal? LifeTime { get; set; }
    //}
    public class LimitResponse : BizResponseClass
    {
        public List<WalletLimitConfigurationRes> WalletLimitConfigurationRes { get; set; }
    }
}
