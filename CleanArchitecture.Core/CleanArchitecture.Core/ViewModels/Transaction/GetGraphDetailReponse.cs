using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal High { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Low { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Open { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Close { get; set; }
        [Range(0, 9999999999.99999999), Column(TypeName = "decimal(18, 8)")]
        public decimal Volume { get; set; }
    }
}
