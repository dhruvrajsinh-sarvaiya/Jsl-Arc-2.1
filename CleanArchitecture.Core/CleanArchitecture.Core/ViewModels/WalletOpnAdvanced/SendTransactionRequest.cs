using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class SendTransaction
    {
        [Required]
        public RoothalfSigned halfSigned { get; set; }
        public string txHex { get; set; }
        [Required]
        public string otp { get; set; }
        public string comment { get; set; }
    }
    //Note : will be defined after live call 
    public class RoothalfSigned
    {

    }
}
