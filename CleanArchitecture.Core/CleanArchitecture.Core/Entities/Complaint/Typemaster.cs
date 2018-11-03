using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Complaint
{
   public class Typemaster : BizBase 
    {
        [Required]
        [StringLength(100)]
        public string Type { get; set; }
        [Required]
        [StringLength(150)]
        public string SubType { get; set; }
        public bool EnableStatus { get; set; }
    }
}
