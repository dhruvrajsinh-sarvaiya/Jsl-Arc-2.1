using CleanArchitecture.Core.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class LoginWithEmailViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //public bool RememberMe { get; set; }

        //public  TrackerViewModel trackerViewModel { get; set; }
    }

    public class LoginWithEmailResponse : BizResponseClass
    {

    }
}
