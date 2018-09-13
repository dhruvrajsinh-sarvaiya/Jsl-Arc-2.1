using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;

namespace CleanArchitecture.Core.Entities
{
    public class ServiceConfiguration : BizBase
    {
        [Key]       
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public short Status { get; set; }
        public short ServiceType { get; set; }
        public decimal MinimumAmount { get; set; }
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
