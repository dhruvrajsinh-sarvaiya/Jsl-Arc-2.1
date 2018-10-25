using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class UserPreferencesRes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? UserID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public short? IsWhitelisting { get; set; }
        public BizResponseClass BizResponse { get; set; }
    }
}
