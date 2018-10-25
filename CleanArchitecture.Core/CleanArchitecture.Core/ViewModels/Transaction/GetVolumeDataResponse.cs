using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetVolumeDataResponse : BizResponseClass
    {
        public List<VolumeDataRespose> response { get; set; }   
    }
    public class VolumeDataRespose
    {
        public long PairId { get; set; }
        public string PairName { get; set; }
        [JsonProperty(PropertyName = "CurrentRate")]
        public Decimal Currentrate { get; set; }
        public decimal Volume24 { get; set; }
        public decimal ChangePer { get; set; }
        public decimal High24Hr { get; set; }
        public decimal Low24Hr { get; set; }
        public decimal HighWeek { get; set; }
        public decimal LowWeek { get; set; }
        public decimal High52Week { get; set; }
        public decimal Low52Week { get; set; }
        public short UpDownBit { get; set; }
    }
}
