using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Core.Entities
{
    public class ProviderConfiguration : BizBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SerProID { get; set; }

        [StringLength(30)]
        public string SerProName { get; set; }
        //public short Status { get; set; }
        [Required]
        public short AppType { get; set; }

        public long APIServiceID { get; set; }

        public void SetActiveProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.Active);
            Events.Add(new ServiceStatusEvent<ProviderConfiguration>(this));
        }
        public void SetInActiveProvider()
        {
            Status = Convert.ToInt16(ServiceStatus.InActive);
            Events.Add(new ServiceStatusEvent<ProviderConfiguration>(this));
        }
    }
}
