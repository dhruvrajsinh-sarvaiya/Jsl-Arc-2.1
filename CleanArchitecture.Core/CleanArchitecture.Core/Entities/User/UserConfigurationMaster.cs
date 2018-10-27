using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.User
{
    public class UserConfigurationMaster : BizBase
    {

        [Required]
        public int UserId { get; set; }

        public string Type { get; set; }

        public string ConfigurationValue  { get; set; }

        public bool EnableStatus { get; set; }


        public void SetAsOTPStatus()
        {
            EnableStatus = true;
            Events.Add(new ServiceStatusEvent<UserConfigurationMaster>(this));
        }

        public void SetAsUpdateDate(long Id)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = Id;
            Events.Add(new ServiceStatusEvent<UserConfigurationMaster>(this));
        }

    }
}
