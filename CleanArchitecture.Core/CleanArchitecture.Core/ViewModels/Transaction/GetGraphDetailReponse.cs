using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetGraphDetailReponse : BizResponseClass
    {
       public List<GetGraphDetailInfo> response { get; set; }
    }
    public class GetGraphDetailInfo
    {
        public long DataDate { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal TodayOpen { get; set; }
        public decimal TodayClose { get; set; }
        public decimal Volume { get; set; }
        public decimal ChangePer { get; set; }
    }
}
