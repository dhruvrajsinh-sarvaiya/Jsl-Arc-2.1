using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendNotificationRequest : IRequest<SendNotificationResponse>
    {
        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [Required]
        public string DeviceID { get; set; }
    }
}
