using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities
{
    public  class CommAPIServiceMaster : BizBase
    {
        [Required]
        public long APID { get; set; }

        [Required]
        public long CommServiceID { get; set; }

        [Required]
        [StringLength(60)]
        public string SenderID { get; set; }

        [Required]
        [StringLength(50)]
        public string SMSSendURL { get; set; }

        [Required]
        [StringLength(50)]
        public string SMSBalURL { get; set; }        

        [Required]
        public int Priority { get; set; }
    }
}
