using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities
{
    public class ServiceConfiguration : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long ServiceID { get; set; }

        [StringLength(30)]
        public string ServiceName { get; set; }
        //public short Status { get; set; }

        [StringLength(10)]
        public string SMSCode { get; set; }
        public short ServiceType { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinimumAmount { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MaximumAmount { get; set; }

        public void SetActiveService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ServiceConfiguration>(this));
        }
        public void SetInActiveService()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<ServiceConfiguration>(this));
        }
    }
}
