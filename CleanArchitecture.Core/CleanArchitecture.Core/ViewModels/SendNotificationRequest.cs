using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.Enums;
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

        [Required]
        [StringLength(200)]
        public string TickerText { get; set; }

        [Required]
        [StringLength(200)]
        public string ContentTitle { get; set; }
    }

    public class DeviceRegistrationRequest : IRequest<CommunicationResponse>
    {
        public long UserID { get; set; }

        public string DeviceID { get; set; }

        public EnDeviceSubsscrptionType SubsscrptionType { get; set; }
}
}
