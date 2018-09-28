using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public  class TransactionStatus
    {
        [Key]
        [Required]
        public long TrnNo { get; set; }
        [Key]
        [Required]
        public long ServiceID { get; set; }
        [Key]
        [Required]
        public long SerProID { get; set; }
        //IsStatusCheck
        public short Status { get; set; }
        public string StatusMsg { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ResponseTime { get; set; }         
    }
}
