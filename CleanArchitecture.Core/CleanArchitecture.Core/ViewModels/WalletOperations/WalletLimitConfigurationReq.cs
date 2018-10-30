using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WalletLimitConfigurationReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameters,4210")]
        [EnumDataType(typeof(enWalletLimitType), ErrorMessage ="1,Fail,4214")]
        public enWalletLimitType TrnType { get; set; }
        //public int TrnType { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4211")]        
        public decimal LimitPerHour { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4212")]
        public decimal LimitPerDay { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4213")]
        public decimal LimitPerTransaction { get; set; }

        //[Required(ErrorMessage = "1,Please Enter Required Parameters,4229")]
        public double StartTime { get; set; }

        //[Required(ErrorMessage = "1,Please Enter Required Parameters,4230")]
        public double EndTime { get; set; }
    }
}
