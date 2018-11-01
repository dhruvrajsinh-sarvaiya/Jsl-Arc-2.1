using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOperations
{
    public class UserPreferencesReq
    {
        [Required(ErrorMessage = "1,Please Enter Required Parameter,4228")]
        public short IsWhitelisting { get; set; }
    }
    public class UserPreferencesRes
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public long? UserID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public short? IsWhitelisting { get; set; }
        public BizResponseClass BizResponse { get; set; }
    }
}
