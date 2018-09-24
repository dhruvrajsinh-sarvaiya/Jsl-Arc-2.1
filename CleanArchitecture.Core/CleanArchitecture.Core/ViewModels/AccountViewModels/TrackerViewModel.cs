using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class TrackerViewModel
    {
        [Required]
        public string MemberType { get; set; }

        [Required]
        public string DeviceId { get; set; }

        [Required]
        public string Mode { get; set; }

        [Required]
        public string IPAddress { get; set; }
    }
}
