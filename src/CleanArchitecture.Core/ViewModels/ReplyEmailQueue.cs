using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class ReplyEmailQueue
    {
        [Required]
        public long RefNo { get; set; }

        [Required]
        [Phone]
        public string MobileNo { get; set; }

        [Required]
        [StringLength(200)]
        public string SMSText { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public short TrnMode { get; set; }

        public string GTalkID { get; set; }

        public int ChannelID { get; set; }
    }
}
