using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class ServiceLimitChargeValueResponse : BizResponseClass
    {
        public ServiceLimitChargeValue response { get; set; }
    }
    public class ServiceLimitChargeValue
    {
        public enTrnType TrnType { get; set; }
        public string CoinName { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal MinAmount { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal MaxAmount { get; set; }
        public enChargeType ChargeType { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal ChargeValue { get; set; }
    }

    public class ServiceLimitChargeValueResponseStr : BizResponseClass
    {
        public ServiceLimitChargeValueStr response { get; set; }
    }

    public class ServiceLimitChargeValueStr
    {
        public enTrnType TrnType { get; set; }
        public string CoinName { get; set; }
        //[Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public string MinAmount { get; set; }
        //[Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public string MaxAmount { get; set; }
        public enChargeType ChargeType { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal ChargeValue { get; set; }
    }
}
