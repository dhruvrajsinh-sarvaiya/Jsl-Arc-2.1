using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities
{
    public class RouteConfiguration : BizBase
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long RouteID { get; set; }

        [Required]
        [StringLength(30)]
        public string RouteName { get; set; }
        //public short Status { get; set; }
        [Required]
        public long ServceID { get; set; }

        [Required]
        public long SerProID { get; set; }

        [Required]
        public long ProductID { get; set; }

        [Required]
        public short Priority { get; set; }

        public string StatusCheckUrl { get; set; }
        public string ValidationUrl { get; set; }
        public string TransactionUrl { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MinimumAmount { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 8)")]
        public decimal MaximumAmount { get; set; }

        [StringLength(50)]
        public string OpCode { get; set; }

        public void SetActiveRoute()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<RouteConfiguration>(this));
        }
        public void SetInActiveRoute()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<RouteConfiguration>(this));
        }
    }
}
