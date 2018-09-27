using CleanArchitecture.Core.ApiModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SignUpWithEmailViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class SignUpWithEmailResponse : BizResponseClass
    {

    }
}
