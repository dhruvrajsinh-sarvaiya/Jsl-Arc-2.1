using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.SharedKernel
{
    public class BizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }

        DateTime CreatedDate { get; set; }

        //DateTime? UpdatedDate { get; set; } already there in basedomainevent

        long CreatedBy { get; set; }

        long? UpdatedBy { get; set; }

        int Status { get; set; }

        public List<BaseDomainEvent> Events = new List<BaseDomainEvent>();
    }
}
