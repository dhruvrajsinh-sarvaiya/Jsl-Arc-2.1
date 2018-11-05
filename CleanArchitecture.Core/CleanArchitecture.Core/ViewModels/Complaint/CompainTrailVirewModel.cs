using CleanArchitecture.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.Complaint
{
    public class CompainTrailVirewModel
    {
        [Required(ErrorMessage = "1,Please Enter complaint description,4119")]
        public string Description { get; set; }
        public string Remark { get; set; }
        [Required(ErrorMessage = "3,Please Enter complaint Complainstatus,4120")]
        public string Complainstatus { get; set; }
        public long ComplainId { get; set; }
        public int UserID { get; set; }

    }
    public class CompainTrailResponse : BizResponseClass
    { }
}
