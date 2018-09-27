using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class AddressMaster : BizBase
    {
        [Required]
        public long WalletMasterId { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public byte IsDefaultAddress { get; set; }

        [Required]
        public long SerProID { get; set; }        
    }

}
