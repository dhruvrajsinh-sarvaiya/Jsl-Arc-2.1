using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class NotificationQueue : BizBase
    {
        [Required]
        public long RefNo { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [StringLength(1000)]
        public string RespText { get; set; }

        [Required]
        public string DeviceID { get; set; }
    }
}
