using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class AddressMasterResponse
    {
        //public long WalletId { get; set; }
        
        public string Address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public byte? IsDefaultAddress { get; set; }
        //public long SerProID { get; set; }
        public string AddressLabel { get; set; }
    }
}
