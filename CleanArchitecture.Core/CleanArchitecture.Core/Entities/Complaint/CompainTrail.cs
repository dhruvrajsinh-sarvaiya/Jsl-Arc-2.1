using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CleanArchitecture.Core.Entities.Complaint
{
  public  class CompainTrail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ComplainId { get; set; }
        [Required]
        [StringLength(4000)]
        public string Description { get; set; }
        [Required]
        [StringLength(2000)]
        public string Remark { get; set; }
        [Required]
        [StringLength(100)]
        public string Complainstatus { get; set; }
        public long? CreatedBy { get; set; }
         
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }

    }
}
