using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.ViewModels.AccountViewModels
{
    public class RegisterViewModel : TrackerViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Firstname")]
        public string Firstname { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Phone]
        public string Mobile { get; set; }

    }
}
