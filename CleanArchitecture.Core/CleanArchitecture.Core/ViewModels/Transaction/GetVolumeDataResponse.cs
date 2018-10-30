using CleanArchitecture.Core.ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public Decimal Currentrate { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Volume24 { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal ChangePer { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal High24Hr { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Low24Hr { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal HighWeek { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal LowWeek { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal High52Week { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Low52Week { get; set; }
        public short UpDownBit { get; set; }
    }
}
