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
    public class MessagingQueue : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long SMSID { get; set; }

        //[Required]
        //public long RefNo { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string SMSText { get; set; }

        [StringLength(1000)]
        public string RespText { get; set; }
        
        public short SMSServiceID { get; set; }
        
        public short SMSSendBy { get; set; }

        //[Required]
        //public short Gateway { get; set; }

        //public short ChannelID { get; set; }

        //public string GTalkID { get; set; }

        //public string RegisterID { get; set; }


        public void FailMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Fail);
            Events.Add(new ServiceStatusEvent<MessagingQueue>(this));
        }

        public void InQueueMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Pending);
            Events.Add(new ServiceStatusEvent<MessagingQueue>(this));
        }

        public void SentMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Success);
            Events.Add(new ServiceStatusEvent<MessagingQueue>(this));
        }
    }
}
