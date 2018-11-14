using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class LimitRuleMasterRes
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public enTrnType TrnType { get; set; }
        public string StrTrnType { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string WalletTypeName { get; set; }
        public long WalletType { get; set; }
        public short Status { get; set; }
        public string StatusStr { get; set; }
    }

    public class ListLimitRuleMasterRes : BizResponseClass
    {
        public List<LimitRuleMasterRes> LimitRuleObj { get; set; }
    }

    public class LimitRuleMasterReq
    {
        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        public string Name { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        [EnumDataType(typeof(enTrnType), ErrorMessage = "1,Invalid Parameter Value,4701")]
        public enTrnType TrnType { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency, ErrorMessage = "1,Enter Required Parameter Value,4700")]
        public decimal MinAmount { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        [Range(0, 9999999999.99999999), DataType(DataType.Currency, ErrorMessage = "1,Enter Required Parameter Value,4700")]
        public decimal MaxAmount { get; set; }

        [Required(ErrorMessage = "1,Enter Required Parameter Value,4700")]
        public long WalletType { get; set; }
    }
}
