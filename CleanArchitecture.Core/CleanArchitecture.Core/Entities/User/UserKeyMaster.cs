using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.User
{
    public class UserKeyMaster : BizBase
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string uniqueKey { get; set; }

        public bool EnableStatus { get; set; }

        public void SetAsUniqueKeyStatus()
        {
            EnableStatus = true;
            Events.Add(new ServiceStatusEvent<UserKeyMaster>(this));
        }

        public void SetAsUpdateDate(long Id)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = Id;
            Events.Add(new ServiceStatusEvent<UserKeyMaster>(this));
        }
    }
}
