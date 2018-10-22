using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Configuration
{
    public class TrackerViewModel
    {
        //[Required]
        //[StringLength(50)]
        //public string MemberType { get; set; }

        [Required(ErrorMessage = "1,DeviceID Not Found,4015")]
        [StringLength(2000, ErrorMessage = "1,DeviceID Not Valid,4016")]
        public string DeviceId { get; set; }

        [Required(ErrorMessage = "1,Mode Not Found,4017")]
        [StringLength(10, ErrorMessage = "1,Mode Not Valid,4018")]
        public string Mode { get; set; }

        [Required(ErrorMessage = "1,IPAddress Not Found,4019")]
        [StringLength(15, ErrorMessage = "1,Invalid IPAddress,4020")]
        public string IPAddress { get; set; }

        [Required(ErrorMessage = "1,HostName Not Found,4021")]
        [StringLength(250, ErrorMessage = "1,Invalid HostName,4022")]
        public string HostName { get; set; }
    }
}
