using CleanArchitecture.Core.ApiModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels
{
    public class EnableAuthenticatorViewModel
    {
        //[Required]
        //[StringLength(7, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long.,4011,", MinimumLength = 6)]
        //[DataType(DataType.Text)]
        //[Display(Name = "Verification Code")]
        //public string Code { get; set; }

        [ReadOnly(true)]
        public string SharedKey { get; set; }

        //public string UserName { get; set; }
        public string AuthenticatorUri { get; set; }
    }

    public class EnableAuthenticationResponse : BizResponseClass
    {
        public EnableAuthenticatorViewModel EnableAuthenticatorViewModel { get; set; }
    }
}
