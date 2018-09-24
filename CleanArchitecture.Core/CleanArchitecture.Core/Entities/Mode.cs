using CleanArchitecture.Core.Enums;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities
{
    public class Mode : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string ModeType { get; set; }
        public bool Status { get; private set; } = false;

        public void EndTimeUpdated()
        {
            Status = Convert.ToBoolean(ModeStatus.True);
            Events.Add(new ModeStatusEvent(this));
        }
    }
}
