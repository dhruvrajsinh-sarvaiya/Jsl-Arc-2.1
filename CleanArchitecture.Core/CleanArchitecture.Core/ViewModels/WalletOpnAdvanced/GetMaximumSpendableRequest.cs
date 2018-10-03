using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.WalletOpnAdvanced
{
    public class GetMaximumSpendableRequest
    {
        public int limit { get; set; }
        public int minValue { get; set; }
        public int maxValue { get; set; }
        public int minHeight { get; set; }
        public int feeRate { get; set; }
        public int maxFeeRate { get; set; }
        public int minConfirms { get; set; }
        public bool enforceMinConfirmsForChange { get; set; }
    }
}
