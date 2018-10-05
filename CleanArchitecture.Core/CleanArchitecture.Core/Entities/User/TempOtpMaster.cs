﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CleanArchitecture.Core.Enums.Modes;
using CleanArchitecture.Core.Events;
using CleanArchitecture.Core.SharedKernel;

namespace CleanArchitecture.Core.Entities.User
{
    public class TempOtpMaster : BizBase
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
        
        public void SetAsOTPStatus()
        {
            Status = Convert.ToInt16(ModeStatus.True);
            Events.Add(new ServiceStatusEvent<TempOtpMaster>(this));
        }
    }
}
