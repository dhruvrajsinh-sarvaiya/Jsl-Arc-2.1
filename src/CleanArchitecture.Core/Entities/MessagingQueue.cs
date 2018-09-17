using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class MessagingQueue : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long SMSID { get; set; }

        [Required]
        public long RefNo { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string SMSText { get; set; }

        [StringLength(1000)]
        public string RespText { get; set; }

        [Required]
        public short SMSServiceID { get; set; }

        [Required]
        public short SMSSendBy { get; set; }

        [Required]
        public short Gateway { get; set; }

        public short ChannelID { get; set; }

        public string GTalkID { get; set; }

        public string RegisterID { get; set; }
    }
}
