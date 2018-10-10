using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Configuration
{
    public class DemonConfiguration : BizBase
    {
        [Required]
        [StringLength(15)]
        public String IPAdd { get; set; }

        [Required]
        public int PortAdd { get; set; }

        [Required]
        [StringLength(200)]
        [DataType(DataType.Url)]
        public string Url { get; set; }

        public void DisableConfiguration()
        {
            Status = Convert.ToInt16(ServiceStatus.Disable);
            Events.Add(new ServiceProviderEvent<DemonConfiguration>(this));
        }
        public void EnableConfiguration()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceProviderEvent<DemonConfiguration>(this));
        }
    }
}
