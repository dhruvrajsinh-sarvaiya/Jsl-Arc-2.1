using CleanArchitecture.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class WalletLimitConfigurationReq
    {
        //[Required(ErrorMessage = "1,Please Enter Required Parameters,4207")]
        //public string AccWalletID { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4207")]
        public enWalletLimitType TrnType { get; set; }
        //public int TrnType { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4207")]
        public decimal LimitPerHour { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4207")]
        public decimal LimitPerDay { get; set; }

        [Required(ErrorMessage = "1,Please Enter Required Parameters,4207")]
        public decimal LimitPerTransaction { get; set; }

    }
}
