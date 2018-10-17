using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.User
{
    public class CustomPassword : BizBase
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Password { get; set; }

        public bool EnableStatus { get; set; }

        public void SetAsPasswordStatus()
        {
            EnableStatus = true;
            Events.Add(new ServiceStatusEvent<CustomPassword>(this));
        }

        public void SetAsUpdateDate(long Id)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = Id;
            Events.Add(new ServiceStatusEvent<CustomPassword>(this));
        }
    }
}
