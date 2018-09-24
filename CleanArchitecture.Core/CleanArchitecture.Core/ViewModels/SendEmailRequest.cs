
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MediatR;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendEmailRequest : IRequest<SendEmailResponse>
    {
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
    }
}
