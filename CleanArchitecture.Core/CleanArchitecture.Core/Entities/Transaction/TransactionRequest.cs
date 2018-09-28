using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public  class TransactionRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public long TrnNo { get; set; }
        public long ServiceID { get; set; }
        public long SerProID { get; set; }
        //IsStatusCheck         
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime RequestTime { get; set; }

        [Required]
        public string RequestData { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime ResponseTime { get; set; }
        
        public string ResponseData { get; set; }
        public short Status { get; set; }
        public string TrnID { get; set; }
        public string OprTrnID { get; set; }
    }
}
