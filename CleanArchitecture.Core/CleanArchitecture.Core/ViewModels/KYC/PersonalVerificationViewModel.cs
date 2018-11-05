using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.KYC
{
    public class PersonalVerificationViewModel : TrackerViewModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
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
        public long KYCLevelId { get; set; }

    }

    public class PersonalVerificationRequest
    {
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string ValidIdentityCard { get; set; }
        public string FrontImage { get; set; }
        public string BackImage { get; set; }
        public string SelfieImage { get; set; }
        public bool EnableStatus { get; set; }
        public bool VerifyStatus { get; set; }
        public long KYCLevelId { get; set; }
        //public  MyProperty { get; set; }
    }

    public class PersonalVerificationResponse : BizResponseClass
    {
        public PersonalVerificationRequest UserKYC { get; set; }
        //public  MyProperty { get; set; }
    }


}
