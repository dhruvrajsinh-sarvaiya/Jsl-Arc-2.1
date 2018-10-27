using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction.BackOffice
{
    public class TradingSummaryRequest
    {
        public long MemberID { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
        
        public long TrnNo { get; set; }

        public short Status { get; set; }

        public string SMSCode { get; set; }

        public String Trade { get; set; }

        public string Pair { get; set; }

        public string MarketType { get; set; }
    }
    public class TradingSummaryResponse : BizResponseClass
    {
        public List<TradingSummaryViewModel> Response { get; set; }
    }
}
