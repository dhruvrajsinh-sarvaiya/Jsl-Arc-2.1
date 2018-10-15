using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.ForgotPassword
{
    public class ForgotPasswordViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,Please Enter Email Id,4007")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid Email Id,4008")]
        //[EmailAddress]
        [RegularExpression(@"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "1,Please enter a valid Email Address,4009")]
        public string Email { get; set; }
    }   

    public class ForgotpassWordResponse  :  BizResponseClass
    {

    }

}
