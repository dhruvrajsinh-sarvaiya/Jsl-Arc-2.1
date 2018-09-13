using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class SMSServiceRequest
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Phone]
        public long MobileNo { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 5)]
        public string Message { get; set; }
    }
}
