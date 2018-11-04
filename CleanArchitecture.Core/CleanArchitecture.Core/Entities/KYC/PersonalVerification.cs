using CleanArchitecture.Core.SharedKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.Entities.KYC
{
    public class PersonalVerification : BizBase
    {

        public int UserID { get; set; }
        [Required]
        [StringLength(150)]
        public string Surname { get; set; }
        [Required]
        [StringLength(150)]
        public string GivenName { get; set; }
        [Required]
        [StringLength(150)]
        public string ValidIdentityCard { get; set; }
        [Required]
        [StringLength(500)]
        public string FrontImage { get; set; }
        [Required]
        [StringLength(500)]
        public string BackImage { get; set; }
        [Required]
        [StringLength(500)]
        public string SelfieImage { get; set; }
        public bool EnableStatus { get; set; }
        public bool VerifyStatus { get; set; }



    }
}
