using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class LoginWithEmailViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //public bool RememberMe { get; set; }

    }
    public class LoginWithEmailresponse : BizResponseClass
    {

    }
}
