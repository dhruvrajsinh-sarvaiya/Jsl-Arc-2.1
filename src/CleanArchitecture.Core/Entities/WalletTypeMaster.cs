using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities
{
    public class WalletTypeMaster : BizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string WalletTypeName { get; set; }
    }
}
