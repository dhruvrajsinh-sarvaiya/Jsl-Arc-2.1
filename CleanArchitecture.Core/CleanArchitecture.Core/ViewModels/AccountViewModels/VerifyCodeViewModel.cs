using CleanArchitecture.Core.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class VerifyCodeViewModel
    {
      
        //public string Provider { get; set; }

        [Required]
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
