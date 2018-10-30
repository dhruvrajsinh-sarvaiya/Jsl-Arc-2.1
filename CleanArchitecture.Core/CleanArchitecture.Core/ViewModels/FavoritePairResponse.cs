using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class FavoritePairResponse : BizResponseClass
    {
        public List<FavouritePairInfo> response { get; set; }
    }
    public class FavouritePairInfo : TradePairRespose
    {
        public string BaseCurrency { get; set; }
        public string BaseAbbrevation { get; set; }
    }
}
