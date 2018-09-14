using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class TransactionParseResponse
    {
        public int Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMsg { get; set; }
        public string OperatorRefNo { get; set; }
        public string TrnRefNo { get; set; }
    }
}
