using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.User
{
    public class OtpMaster
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


        public ApplicationUser User { get; set; }
        public RegisterType RegisterType { get; set; }
    }
}
