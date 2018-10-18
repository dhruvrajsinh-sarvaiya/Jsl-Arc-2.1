using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TransactionTypeResponse : BizResponseClass
    {
        public List<TransactionTypeInfo> response { get; set; }
    }
    public class TransactionTypeInfo
    {
        public long Id { get; set; }
        public string TrnTypeName { get; set; }
    }
}
