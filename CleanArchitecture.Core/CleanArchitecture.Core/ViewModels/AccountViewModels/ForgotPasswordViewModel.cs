using CleanArchitecture.Core.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class ForgotPasswordViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ForgotPasswordResponse : BizResponseClass
    {

    }

}
