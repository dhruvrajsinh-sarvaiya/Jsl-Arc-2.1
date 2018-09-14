using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.SharedKernel
{
    public abstract class BizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public long CreatedBy { get; set; }
               
        public long? UpdatedBy { get; set; }

        public short Status { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();

    }
}
