using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;

namespace CleanArchitecture.Core.Entities
{
    public class ProviderConfiguration : BizBase
    {
        [Key]       
        public long SerProID { get; set; }
        public string SerProName { get; set; }
        public short Status { get; set; }
        public short ServiceType { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }

        public void SetActiveProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ProviderConfiguration>(this));
        }
        public void SetInActiveProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<ProviderConfiguration>(this));
        }
    }
}
