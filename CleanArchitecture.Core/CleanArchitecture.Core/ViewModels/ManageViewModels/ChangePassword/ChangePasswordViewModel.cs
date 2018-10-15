using CleanArchitecture.Core.ApiModels;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.ManageViewModels
{
    public class ChangePasswordViewModel
    {



        [Required(ErrorMessage = "1,Please Enter Password,4010")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long,4011", MinimumLength = 6)]
        [Display(Name = "Current Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "1,Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z) lower case (a-z) number (0-9) and special character (e.g. !@#$%^&*),4028")]
        public string OldPassword { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = " 1, The {0} must be at least {2} and at max {1} characters long.,4024", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "New password")]

        [Required(ErrorMessage = "1,Please Enter Password,4010")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long,4011", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "1,Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z) lower case (a-z) number (0-9) and special character (e.g. !@#$%^&*),4028")]

        public string NewPassword { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm new password")]
        //[Compare("NewPassword", ErrorMessage = "2,The new password and confirmation password do not match.,4027")]

        [Required(ErrorMessage = "1,Please Enter Password,4011")]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "1,The {0} must be at least {2} and at max {1} characters long,4011", MinimumLength = 6)]
        [Display(Name = "Password")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$", ErrorMessage = "1,Passwords must be at least 6 characters and contain at 3 of 4 of the following: upper case (A-Z) lower case (a-z) number (0-9) and special character (e.g. !@#$%^&*),4028")]

        public string ConfirmPassword { get; set; }
    }
    public class ChangePasswordResponse : BizResponseClass
    {

    }
}
