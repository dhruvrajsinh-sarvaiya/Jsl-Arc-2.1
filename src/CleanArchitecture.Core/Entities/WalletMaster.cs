using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.SharedKernel;


namespace CleanArchitecture.Core.Entities
{
    public class WalletMaster : BizBase
    {      

        public decimal Balance { get; set; }
        
        public long WalletTypeID { get; set; }

        public long IsValid { get; set; }
    }

}
