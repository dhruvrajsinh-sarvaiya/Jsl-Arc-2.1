using CleanArchitecture.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ApiModels
{
    class OrderDeliveryRequest : BizRequestClass
    {
        [Required]
        public long OrderID { get; set; }

        [Required]
        [StringLength(150)]
        public string DRemarks { get; set; }
    }
}
