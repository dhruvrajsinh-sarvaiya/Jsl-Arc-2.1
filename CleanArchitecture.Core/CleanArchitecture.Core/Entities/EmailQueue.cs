using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class EmailQueue : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long EmailID { get; set; }

        //public long RefNo { get; set; }

        [Required]
        [StringLength(50)]
        public string Recepient { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string CC { get; set; }

        [StringLength(500)]
        public string BCC { get; set; }

        [StringLength(500)]
        public string Attachment { get; set; }

        public short SendBy { get; set; }

        public short EmailType { get; set; }

        //public short ChannelID { get; set; }


        public void FailMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Fail);
            Events.Add(new ServiceStatusEvent<EmailQueue>(this));
        }

        public void InQueueMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Pending);
            Events.Add(new ServiceStatusEvent<EmailQueue>(this));
        }

        public void SentMessage()
        {
            Status = Convert.ToInt16(MessageStatusType.Success);
            Events.Add(new ServiceStatusEvent<EmailQueue>(this));
        }
    }
}
