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
        public long WalletId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        public byte IsDefaultAddress { get; set; }

        [Required]
        public long SerProID { get; set; }
        
        // Removed as it is no longer required. By -Nishit Jani on B 2018-10-11 6:45.
        //[Required]
        //[StringLength(5)]
        //public string CoinName { get; set; }
        [Required]
        [StringLength(50)]
        public string AddressLable { get; set; }


    }

}
