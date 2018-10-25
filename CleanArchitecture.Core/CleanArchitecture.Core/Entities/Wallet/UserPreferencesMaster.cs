using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.Wallet
{
    public class UserPreferencesMaster : BizBase
    {
        [Required]
        public long UserID { get; set; }
        public short IsWhitelisting { get; set; }
    }
}
