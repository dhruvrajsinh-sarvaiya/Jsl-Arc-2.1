using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class SocialLoginWithMobileViewModel
    {
        [Required]
        [Phone]
        public string Mobile { get; set; }

        //public string ProviderKey { get; set; }

        //public string ProviderName { get; set; }

        //public int UserID { get; set; }

    }
}
