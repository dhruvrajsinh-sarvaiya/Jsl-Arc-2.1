using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.Login
{
    public class SocialLoginWithGoogleViewModel : TrackerViewModel
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "Please enter a valid Email Address")]
        public string Email { get; set; }
        [Required]
        public string ProviderKey { get; set; }
        [Required]
        public string ProviderName { get; set; }
        [Required]
        public string access_token { get; set; }
        //public string refresh_token { get; set; }
        //public string token_type { get; set; }
        //public string expires_at { get; set; }
        //public string TicketCreated { get; set; }
    }

    public class SocialLoginGoogleResponse : BizResponseClass
    {

    }

}
