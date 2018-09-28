using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TrackerViewModel
    {
        [Required]
        public string MemberType { get; set; }

        [Required]
        [StringLength(20)]
        public string DeviceId { get; set; }

        [Required]
        [StringLength(10)]
        public string Mode { get; set; }

        [Required]
        [StringLength(15)]
        public string IPAddress { get; set; }

        [Required]
        [StringLength(250)]
        public string HostName { get; set; }
    }
}
