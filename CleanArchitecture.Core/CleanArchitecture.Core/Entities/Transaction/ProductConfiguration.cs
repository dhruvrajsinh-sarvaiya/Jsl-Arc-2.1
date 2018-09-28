using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities
{
    public class ProductConfiguration : BizBase
    {
       // [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long ProductID { get; set; }

        [Required]
        [StringLength(30)]
        public string ProductName { get; set; }
        //public short Status { get; set; }
        [Required]
        public long ServceID { get; set; }

        [Required]
        public long StateID { get; set; }        

        public void SetActiveProduct()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ProductConfiguration>(this));
        }
        public void SetInActiveProduct()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<ProductConfiguration>(this));
        }
    }
}
