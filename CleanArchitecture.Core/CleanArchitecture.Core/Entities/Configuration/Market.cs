using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.Entities.Communication
{
    public class Market : BizBase
    {
        public string CurrencyName { get; set; }
        public short isBaseCurrency { get; set; }
    }
}
