using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class FreezeWalletRequest
    {
        public int duration { get; set; }
        [Required]
        public string otp { get; set;}
    }
}
