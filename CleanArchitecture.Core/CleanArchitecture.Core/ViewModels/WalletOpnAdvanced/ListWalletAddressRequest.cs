using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletAddressRequest
    {
        [Required]
        public string id { get; set; }

        public int limit { get; set; }
        public string prevId { get; set; }
        public int sortOrder { get; set; }
        public string labelContains { get; set; }
    }
}
