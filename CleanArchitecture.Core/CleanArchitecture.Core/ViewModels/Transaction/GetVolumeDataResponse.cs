using CleanArchitecture.Core.ApiModels;
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
        public Decimal Currentrate { get; set; }
        public decimal Volume24 { get; set; }
        public decimal ChangePer { get; set; }
    }
}
