using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Core.Enums;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class ServiceProviderMaster :BizBase
    {
        [Required]
        [StringLength(60)]
        public string ProviderName { get; set; }

        public void DisableProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceProviderEvent<ServiceProviderMaster>(this));
        }

        public void EnableProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceProviderEvent<ServiceProviderMaster>(this));
        }
    }
}
