using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ServiceStastics : BizBase
    {
        public long ServiceId { get; set; }

        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MarketCap { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal VolGlobal { get; set; }
        public long MaxSupply { get; set; }
        public long CirculatingSupply { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        public decimal IssuePrice { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
