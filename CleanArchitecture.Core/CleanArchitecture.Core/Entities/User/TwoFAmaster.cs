﻿using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.User
{
    public class TwoFAmaster : BizBase
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RegTypeId { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        [Range(6, Int64.MaxValue)]
        public string OTP { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ExpirTime { get; set; }
        public bool EnableStatus { get; set; }

        public void SetAsOTPStatus()
        {
            EnableStatus = true;
            Events.Add(new ServiceStatusEvent<TwoFAmaster>(this));
        }

        public void SetAsUpdateDate(long Id)
        {
            UpdatedDate = DateTime.UtcNow;
            UpdatedBy = Id;
            Events.Add(new ServiceStatusEvent<TwoFAmaster>(this));
        }
    }
}