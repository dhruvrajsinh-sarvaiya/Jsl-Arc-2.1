using CleanArchitecture.Core.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class VerifyCodeViewModel
    {

        //public string Provider { get; set; }

        [Required(ErrorMessage = "1,Please enter a twofactore authentication code,4065")]
        public string Code { get; set; }

        //public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
    public class VerifyCodeResponse : BizResponseClass
    {

    }
}
