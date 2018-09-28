using CleanArchitecture.Core.ViewModels.Configuration;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class SocialLoginWithEmailViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //public string ProviderKey { get; set; }

        public string ProviderName { get; set; }

        //public int UserID { get; set; }
    }
}
