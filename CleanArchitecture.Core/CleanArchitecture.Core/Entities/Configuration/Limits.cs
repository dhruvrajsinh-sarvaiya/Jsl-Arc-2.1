using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class Limits : BizBase
    {
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinAmt { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MaxAmt { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinAmtDaily { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MaxAmtDaily { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinAmtWeekly { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MaxAmtWeekly { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinAmtMonthly { get; set; }
        [Range(0, 9999999999.99999999), DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public long MaxAmtMonthly { get; set; }
        public long MinRange { get; set; }
        public long Maxrange { get; set; }
        public long MinRangeDaily { get; set; }
        public long MaxRangeDaily { get; set; }
        public long MinRangeWeekly { get; set; }
        public long MaxRangeWeekly { get; set; }
        public long MinRangeMonthly { get; set; }
        public long MaxRangeMonthly { get; set; }
        public void SetActiveLimit()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<Limits>(this));
        }
        public void SetInActiveLimit()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<Limits>(this));
        }
    }
}
