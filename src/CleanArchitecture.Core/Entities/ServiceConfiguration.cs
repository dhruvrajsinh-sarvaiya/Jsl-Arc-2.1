using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class ServiceConfiguration : BizBase
    {
        [Key]       
        public long ServiceID { get; set; }
        public string ServiceName { get; set; }
        public short Status { get; set; }
        public short ServiceType { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }

        public void SetActiveService()
        {
            Status = 1;
            Events.Add(new ServiceActiveInactiveEvent(this));
        }
        public void SetInActiveService()
        {
            Status = 9;
            Events.Add(new ServiceActiveInactiveEvent(this));
        }
    }
}
