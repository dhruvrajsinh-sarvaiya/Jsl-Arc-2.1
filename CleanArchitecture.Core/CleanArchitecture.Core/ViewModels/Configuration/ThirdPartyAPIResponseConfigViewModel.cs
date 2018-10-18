using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class ThirdPartyAPIResponseConfigViewModel
    {
        public long Id { get; set; }

        public string BalanceRegex { get; set; }

        public string StatusRegex { get; set; }

        public string StatusMsgRegex { get; set; }

        public string ResponseCodeRegex { get; set; }

        public string ErrorCodeRegex { get; set; }

        public string TrnRefNoRegex { get; set; }

        public string OprTrnRefNoRegex { get; set; }

        public string Param1Regex { get; set; }

        public string Param2Regex { get; set; }

        public string Param3Regex { get; set; }
    }
}
