using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels
{
    public class CreateOrderRequest : CommonRequestClass
    {
        [Required]
        public decimal OrderAmt { get; set; }

        [Required]
        [StringLength(150)]
        public string ORemarks { get; set; }

        [Required]
        public long OWalletMasterID { get; set; }
    }

    public class CreateOrderResponse : CommonResponseClass
    {
        public decimal OrderID { get; set; }

        public string ORemarks { get; set; }        
    }
}
