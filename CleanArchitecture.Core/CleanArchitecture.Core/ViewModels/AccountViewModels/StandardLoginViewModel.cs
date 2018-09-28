using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class StandardLoginViewModel : TrackerViewModel
    {
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }
    }

    public class StandardLoginResponse : BizResponseClass
    {
        public string MemberDetails { get; set; }
        public bool IsOTP { get; set; }
        public bool Is2FA { get; set; }
        public bool IsEmailVerified { get; set; }
        public bool IsListedIP { get; set; }
        public bool IsTrustedHost { get; set; }
        public bool IsSecurityMessage { get; set; }
        public bool IsSecurityImage { get; set; }
        public string SecurityMessage { get; set; }
        public int SecurityImageId { get; set; }
        public int LoginAttemptCount { get; set; }
        public decimal Balance { get; set; }
        public string TwoFAQRKey { get; set; }
        public string TwoFAAuthCode { get; set; }
        public bool IsDefaultSetup { get; set; }
        public int SelectedDefaultPageID { get; set; }
        public int MaxImagePostCount { get; set; }
        public int ProfileID { get; set; }
    }
}
