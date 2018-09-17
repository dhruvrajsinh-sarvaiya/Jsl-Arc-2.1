using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class ServiceTypeMaster : BizBase
    {
        [Required]
        public long ServiceTypeID { get; set; }

        [Required]
        [StringLength(60)]
        public string ServiceTypeName { get; set; }

        public void DisableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceStatusEvent<ServiceTypeMaster>(this));
        }

        public void EnableService()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ServiceTypeMaster>(this));
        }
    }
}
