using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendNotificationRequest : IRequest<CommunicationResponse>
    {
        [StringLength(50)]
        public string Subject { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [Required]
        [StringLength(500)]
        public string DeviceID { get; set; }
    }
}
