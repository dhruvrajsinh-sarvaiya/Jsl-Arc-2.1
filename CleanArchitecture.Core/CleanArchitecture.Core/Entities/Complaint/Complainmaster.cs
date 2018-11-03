using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Complaint
{
   public class Complainmaster  : BizBase
    {
        public int UserID { get; set; }
        public long TypeId { get; set; }
        [Required]
        [StringLength(500)]
        public string Subject { get; set; }
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
    }
}
