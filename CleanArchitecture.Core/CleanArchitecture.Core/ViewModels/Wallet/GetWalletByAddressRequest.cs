using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Wallet
{
    public class GetWalletByAddressRequest
    {
        [Required]
        public string address { get; set; }
    }
}
