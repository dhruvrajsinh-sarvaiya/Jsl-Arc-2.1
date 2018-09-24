using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SendSMSRequest : IRequest<SendSMSResponse>
    {
        [Required]
        public int TemplateTypeID { get; set; }

        [Required]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Message { get; set; }
        
        public long CommServiceTypeID { get; set; }
    }
}
