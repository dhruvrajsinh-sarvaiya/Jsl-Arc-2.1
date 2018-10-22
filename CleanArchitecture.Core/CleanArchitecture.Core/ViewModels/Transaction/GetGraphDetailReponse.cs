using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Transaction
{
    public class GetGraphDetailReponse : BizResponseClass
    {
       public GetGraphDetailInfo response { get; set; }
    }
    public class GetGraphDetailInfo
    {
        public List<decimal[]> GraphData { get; set; }
    }
}
