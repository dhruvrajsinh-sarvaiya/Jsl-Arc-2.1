using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.Enums;

namespace CleanArchitecture.Core.Entities
{
    public class NotificationQueue : BizBase
    {
        //[Required]
        //public long RefNo { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string Message { get; set; }

        [Required]
        public string DeviceID { get; set; }


        public void FailMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Fail);
            Events.Add(new ServiceStatusEvent<NotificationQueue>(this));
        }

        public void InQueueMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Pending);
            Events.Add(new ServiceStatusEvent<NotificationQueue>(this));
        }

        public void SentMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Success);
            Events.Add(new ServiceStatusEvent<NotificationQueue>(this));
        }
    }
}
