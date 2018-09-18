using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    public class SMSServiceRequest
    {
        [Required]
        public int TemplateID { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Message { get; set; }
    }
}
