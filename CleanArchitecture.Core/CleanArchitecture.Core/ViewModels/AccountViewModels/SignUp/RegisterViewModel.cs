using CleanArchitecture.Core.ApiModels;
using CleanArchitecture.Core.ViewModels.Configuration;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels.SignUp
{
    public class RegisterViewModel : TrackerViewModel
    {
        [Required(ErrorMessage = "1,Please enter a User Name,4001")]
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "1,Please enter a valid User Name,4002")]
        public string Username { get; set; }

        [Required(ErrorMessage = "1,Please Enter First Name,4003")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid First Name,4004")]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "1,Please Enter Last Name,4005")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid Last Name,4006")]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "1,Please Enter Email Id,4007")]
        [StringLength(50, ErrorMessage = "1,Please Enter Valid Email Id,4008")]
        [RegularExpression(@"^[-a-z0-9~!$%^&*_=+}{\'?]+(\.[-a-z0-9~!$%^&*_=+}{\'?]+)*@([a-z0-9_][-a-z0-9_]*(\.[-a-z0-9_]+)*\.(aero|arpa|biz|com|coop|edu|gov|info|int|mil|museum|name|net|org|pro|travel|mobi|[a-z][a-z])|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,5})?$", ErrorMessage = "1,Please enter a valid Email Address,4009")]        
        //[EmailAddress(ErrorMessage ="1,Please Enter Valid Email Id, 4009")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "1,Please Enter Password,4010")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long,4011", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "1,Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z) lower case (a-z) number (0-9) and special character (e.g. !@#$%^&*),4028")]
        public string Password { get; set; }

        [Required(ErrorMessage = "1,Please Enter Mobile Number,4012")]
        // [Phone(ErrorMessage = "1,Please Enter Valid Mobile Number,4013")]
        //  [StringLength(10, MinimumLength = 10, ErrorMessage = "1,Please Enter Valid Mobile Number,4014")]
        //  [Range(10, Int64.MaxValue)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }
    }

    public class RegisterResponse : BizResponseClass
    {

    }


}
