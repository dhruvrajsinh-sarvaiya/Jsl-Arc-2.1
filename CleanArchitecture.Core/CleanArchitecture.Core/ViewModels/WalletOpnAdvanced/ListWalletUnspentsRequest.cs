using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class ListWalletUnspentsRequest
    {
        public string prevId { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int minHeight { get; set; }
        public int minConfirms { get; set; }
    }
}
